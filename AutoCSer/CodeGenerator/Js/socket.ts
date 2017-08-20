/// <reference path = "./base.page.ts" />
'use strict';
module AutoCSer {
    class SocketBuffer {
        Data: number[];
        constructor() {
            this.Data = [];
        }
        Byte(Value: number) {
            this.Data.push(Value);
        }
        Short(Value: number) {
            if (Value >= 0) this.UShort(Value);
            else {
                Value = -Value;
                this.Data.push(Value & 255);
                this.Data.push((Value >> 8) | (1 << 7));
            }
        }
        UShort(Value: number) {
            this.Data.push(Value & 255);
            this.Data.push(Value >> 8);
        }
        Int(Value: number) {
            if (Value >= 0) this.UInt(Value);
            else {
                Value = -Value;
                this.UShort(Value & 65535);
                this.UShort((Value >> 16) | (1 << 15));
            }
        }
        UInt(Value: number) {
            this.UShort(Value & 65535);
            this.UShort(Value >> 16);
        }
        UShortString(Value: string) {
            if (Value.length < 65536) {
                this.UShort(Value.length);
                if (Value.length) this.PushString(Value, Pub.ThisFunction(this, this.UShort));
            }
        }
        String(Value: string) {
            if (Value.length < (1 << 15)) {
                if (Value.length) this.UShortString(Value);
            }
            else {
                this.UShort((Value.length & 65535) | (1 << 15));
                this.UShort(Value.length >> 15);
                this.PushString(Value, Pub.ThisFunction(this, Value.length < 65536 ? this.UShort : this.UInt));
            }
        }
        PushString(Value: string, PushLength: Function) {
            for (var Data = [], Index = 0; Index - Value.length;) {
                for (Data.length = 0; Index - Value.length; ++Index) {
                    var Code = Value.charCodeAt(Index);
                    if (Code >= 256) break;
                    Data.push(Code);
                }
                PushLength(Data.length);
                this.Data = this.Data.concat(Data);
                if (Index - Value.length) {
                    for (Data.length = 0; Index - Value.length; ++Index) {
                        var Code = Value.charCodeAt(Index);
                        if (Code < 256) break;
                        Data.push(Code & 255);
                        Data.push(Code >> 8);
                    }
                    PushLength(Data.length >> 1);
                    this.Data = this.Data.concat(Data);
                }
            }
        }
        StringByte(Value: string) {
            for (var Index = 0; Index - Value.length; this.Data.push(Value.charCodeAt(Index++)));
        }
        GetBlob() {
            return new Blob([new Uint8Array(this.Data).buffer]);
        }
    }
    export class Socket {
        private static DefaultParameter = { Url: '/', DataType: 'arraybuffer', AutoTimeout: 0, PingData: null, PingTimeout: 50 * 1000 };
        private static DefaultEvents = { OnOpen: null, OnClose: null, OnError: null, OnData: null, OnString: null };
        private Url: string;
        private DataType: string;
        private AutoTimeout: number;
        private PingData: number[];
        private PingTimeout: number;

        private OnOpen: Events;
        private OnClose: Events;
        private OnError: Events;
        private OnData: Events;
        private OnString: Events;

        private Identity: number;
        private IsQueue: number;
        private IsOpen: number;
        private PingBuffer: SocketBuffer;
        private PingTime: Date;
        private WebSocket: WebSocket;
        private Queue: SocketBuffer[];
        private IsClose: boolean;
        constructor(Parameter) {
            Pub.GetParameter(this, Socket.DefaultParameter, Parameter);
            Pub.GetEvents(this, Socket.DefaultEvents, Parameter);
            if (this.Url.indexOf('://') == -1) {
                if (this.Url.substring(0, 2) == '//') this.Url = 'ws' + location.protocol.substring(4) + this.Url;
                else this.Url = 'ws' + location.protocol.substring(4) + '//' + location.hostname + (this.Url.charCodeAt(0) == 47 ? this.Url : ('/' + this.Url));
            }
            this.Identity = this.IsQueue = 0;
            this.Queue = [];
            if (this.PingData) {
                (this.PingBuffer = new SocketBuffer()).Data = this.PingData;
                this.PingTime = (new Date).AddMilliseconds(this.PingTimeout);
                setTimeout(Pub.ThisFunction(this, this.Ping), this.PingTimeout);
            }
            if (this.AutoTimeout) this.Create();
        }
        private Create() {
            if (!this.IsClose && !this.IsOpen) {
                ++this.Identity;
                this.IsOpen = 1;
                this.WebSocket = new WebSocket(this.Url);
                this.WebSocket.binaryType = this.DataType;
                this.WebSocket.onopen = Pub.ThisFunction(this, this.Open, [this.Identity]) as (Event: Event) => any;
                this.WebSocket.onclose = Pub.ThisFunction(this, this.Close, [this.Identity]) as (Event: CloseEvent) => any;
                this.WebSocket.onerror = Pub.ThisFunction(this, this.Error, [this.Identity]) as (Event: Event) => any;
                this.WebSocket.onmessage = Pub.ThisFunction(this, this.Message, [this.Identity]) as (Event: MessageEvent) => any;
            }
        }
        private CheckIdentity(Values: IArguments) {
            for (var Index = Values.length; Index;)	if (Values[--Index] == this.Identity) return 1;
        }
        private CloseSocket(IsClose = false) {
            ++this.Identity;
            this.IsOpen = 0;
            if (!IsClose && this.WebSocket) this.WebSocket.close();
            this.WebSocket = null;
            if (this.AutoTimeout) setTimeout(Pub.ThisFunction(this, this.Create), this.AutoTimeout);
        }
        private Ping() {
            var Timeout = this.PingTime.getTime() - (new Date).getTime();
            if (Timeout <= 0) {
                setTimeout(Pub.ThisFunction(this, this.Ping), this.PingTimeout);
                this.Send(this.PingBuffer, true);
            }
            else setTimeout(Pub.ThisFunction(this, this.Ping), Timeout + 1);
        }
        private Open() {
            if (this.CheckIdentity(arguments)) {
                this.IsOpen = 2;
                this.PingTime = (new Date).AddMilliseconds(this.PingTimeout);
                this.OnOpen.Function(this);
                ++this.IsQueue;
                setTimeout(Pub.ThisFunction(this, this.SendQueue), 500);
            }
        }
        private Close() {
            if (arguments.length == 0) {
                this.IsClose = true;
                this.CloseSocket();
            }
            else {
                if (this.CheckIdentity(arguments)) this.CloseSocket(true);
                this.OnClose.Function(this);
            }
        }
        private Error() {
            if (this.CheckIdentity(arguments)) this.CloseSocket();
            this.OnError.Function(this);
        }
        private Message(Data: MessageEvent) {
            if (this.CheckIdentity(arguments)) {
                if (typeof (Data.data) == 'string') this.OnString.Function(Data.data);
                else this.OnData.Function(Data.data);
            }
        }
        private SendQueue() {
            if (!this.IsClose && --this.IsQueue == 0 && this.IsOpen == 2) {
                var Queue = this.Queue;
                this.Queue = [];
                for (var Index = 0; Index - Queue.length; ++Index) {
                    var Value = Queue[Index];
                    this.WebSocket.send(Value.GetBlob ? Value.GetBlob() : Value);
                }
                this.PingTime = (new Date).AddMilliseconds(this.PingTimeout);
            }
        }
        private Send(Value: SocketBuffer, IsPing = false) {
            if (!this.IsClose) {
                if (this.IsOpen == 2) {
                    if (this.IsQueue) {
                        if (!IsPing) this.Queue.push(Value);
                    }
                    else {
                        this.WebSocket.send(Value.GetBlob ? Value.GetBlob() : Value);
                        this.PingTime = (new Date).AddMilliseconds(this.PingTimeout);
                    }
                }
                else if (!IsPing) {
                    this.Queue.push(Value);
                    if (!this.WebSocket && !this.AutoTimeout) this.Create();
                }
            }
        }
        private CreateBuffer() { return new SocketBuffer(); }
    }
}