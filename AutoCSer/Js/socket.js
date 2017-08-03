/// <reference path = "./base.page.ts" />
'use strict';
var AutoCSer;
(function (AutoCSer) {
    var SocketBuffer = (function () {
        function SocketBuffer() {
            this.Data = [];
        }
        SocketBuffer.prototype.Byte = function (Value) {
            this.Data.push(Value);
        };
        SocketBuffer.prototype.Short = function (Value) {
            if (Value >= 0)
                this.UShort(Value);
            else {
                Value = -Value;
                this.Data.push(Value & 255);
                this.Data.push((Value >> 8) | (1 << 7));
            }
        };
        SocketBuffer.prototype.UShort = function (Value) {
            this.Data.push(Value & 255);
            this.Data.push(Value >> 8);
        };
        SocketBuffer.prototype.Int = function (Value) {
            if (Value >= 0)
                this.UInt(Value);
            else {
                Value = -Value;
                this.UShort(Value & 65535);
                this.UShort((Value >> 16) | (1 << 15));
            }
        };
        SocketBuffer.prototype.UInt = function (Value) {
            this.UShort(Value & 65535);
            this.UShort(Value >> 16);
        };
        SocketBuffer.prototype.UShortString = function (Value) {
            if (Value.length < 65536) {
                this.UShort(Value.length);
                if (Value.length)
                    this.PushString(Value, AutoCSer.Pub.ThisFunction(this, this.UShort));
            }
        };
        SocketBuffer.prototype.String = function (Value) {
            if (Value.length < (1 << 15)) {
                if (Value.length)
                    this.UShortString(Value);
            }
            else {
                this.UShort((Value.length & 65535) | (1 << 15));
                this.UShort(Value.length >> 15);
                this.PushString(Value, AutoCSer.Pub.ThisFunction(this, Value.length < 65536 ? this.UShort : this.UInt));
            }
        };
        SocketBuffer.prototype.PushString = function (Value, PushLength) {
            for (var Data = [], Index = 0; Index - Value.length;) {
                for (Data.length = 0; Index - Value.length; ++Index) {
                    var Code = Value.charCodeAt(Index);
                    if (Code >= 256)
                        break;
                    Data.push(Code);
                }
                PushLength(Data.length);
                this.Data = this.Data.concat(Data);
                if (Index - Value.length) {
                    for (Data.length = 0; Index - Value.length; ++Index) {
                        var Code = Value.charCodeAt(Index);
                        if (Code < 256)
                            break;
                        Data.push(Code & 255);
                        Data.push(Code >> 8);
                    }
                    PushLength(Data.length >> 1);
                    this.Data = this.Data.concat(Data);
                }
            }
        };
        SocketBuffer.prototype.StringByte = function (Value) {
            for (var Index = 0; Index - Value.length; this.Data.push(Value.charCodeAt(Index++)))
                ;
        };
        SocketBuffer.prototype.GetBlob = function () {
            return new Blob([new Uint8Array(this.Data).buffer]);
        };
        return SocketBuffer;
    }());
    var Socket = (function () {
        function Socket(Parameter) {
            AutoCSer.Pub.GetParameter(this, Socket.DefaultParameter, Parameter);
            AutoCSer.Pub.GetEvents(this, Socket.DefaultEvents, Parameter);
            if (this.Url.indexOf('://') == -1) {
                if (this.Url.substring(0, 2) == '//')
                    this.Url = 'ws' + location.protocol.substring(4) + this.Url;
                else
                    this.Url = 'ws' + location.protocol.substring(4) + '//' + location.hostname + (this.Url.charCodeAt(0) == 47 ? this.Url : ('/' + this.Url));
            }
            this.Identity = this.IsQueue = 0;
            this.Queue = [];
            if (this.PingData) {
                (this.PingBuffer = new SocketBuffer()).Data = this.PingData;
                this.PingTime = (new Date).AddMilliseconds(this.PingTimeout);
                setTimeout(AutoCSer.Pub.ThisFunction(this, this.Ping), this.PingTimeout);
            }
            if (this.AutoTimeout)
                this.Create();
        }
        Socket.prototype.Create = function () {
            if (!this.IsClose && !this.IsOpen) {
                ++this.Identity;
                this.IsOpen = 1;
                this.WebSocket = new WebSocket(this.Url);
                this.WebSocket.binaryType = this.DataType;
                this.WebSocket.onopen = AutoCSer.Pub.ThisFunction(this, this.Open, [this.Identity]);
                this.WebSocket.onclose = AutoCSer.Pub.ThisFunction(this, this.Close, [this.Identity]);
                this.WebSocket.onerror = AutoCSer.Pub.ThisFunction(this, this.Error, [this.Identity]);
                this.WebSocket.onmessage = AutoCSer.Pub.ThisFunction(this, this.Message, [this.Identity]);
            }
        };
        Socket.prototype.CheckIdentity = function (Values) {
            for (var Index = Values.length; Index;)
                if (Values[--Index] == this.Identity)
                    return 1;
        };
        Socket.prototype.CloseSocket = function (IsClose) {
            if (IsClose === void 0) { IsClose = false; }
            ++this.Identity;
            this.IsOpen = 0;
            if (!IsClose && this.WebSocket)
                this.WebSocket.close();
            this.WebSocket = null;
            if (this.AutoTimeout)
                setTimeout(AutoCSer.Pub.ThisFunction(this, this.Create), this.AutoTimeout);
        };
        Socket.prototype.Ping = function () {
            var Timeout = this.PingTime.getTime() - (new Date).getTime();
            if (Timeout <= 0) {
                setTimeout(AutoCSer.Pub.ThisFunction(this, this.Ping), this.PingTimeout);
                this.Send(this.PingBuffer, true);
            }
            else
                setTimeout(AutoCSer.Pub.ThisFunction(this, this.Ping), Timeout + 1);
        };
        Socket.prototype.Open = function () {
            if (this.CheckIdentity(arguments)) {
                this.IsOpen = 2;
                this.PingTime = (new Date).AddMilliseconds(this.PingTimeout);
                this.OnOpen.Function(this);
                ++this.IsQueue;
                setTimeout(AutoCSer.Pub.ThisFunction(this, this.SendQueue), 500);
            }
        };
        Socket.prototype.Close = function () {
            if (arguments.length == 0) {
                this.IsClose = true;
                this.CloseSocket();
            }
            else {
                if (this.CheckIdentity(arguments))
                    this.CloseSocket(true);
                this.OnClose.Function(this);
            }
        };
        Socket.prototype.Error = function () {
            if (this.CheckIdentity(arguments))
                this.CloseSocket();
            this.OnError.Function(this);
        };
        Socket.prototype.Message = function (Data) {
            if (this.CheckIdentity(arguments)) {
                if (typeof (Data.data) == 'string')
                    this.OnString.Function(Data.data);
                else
                    this.OnData.Function(Data.data);
            }
        };
        Socket.prototype.SendQueue = function () {
            if (!this.IsClose && --this.IsQueue == 0 && this.IsOpen == 2) {
                var Queue = this.Queue;
                this.Queue = [];
                for (var Index = 0; Index - Queue.length; ++Index) {
                    var Value = Queue[Index];
                    this.WebSocket.send(Value.GetBlob ? Value.GetBlob() : Value);
                }
                this.PingTime = (new Date).AddMilliseconds(this.PingTimeout);
            }
        };
        Socket.prototype.Send = function (Value, IsPing) {
            if (IsPing === void 0) { IsPing = false; }
            if (!this.IsClose) {
                if (this.IsOpen == 2) {
                    if (this.IsQueue) {
                        if (!IsPing)
                            this.Queue.push(Value);
                    }
                    else {
                        this.WebSocket.send(Value.GetBlob ? Value.GetBlob() : Value);
                        this.PingTime = (new Date).AddMilliseconds(this.PingTimeout);
                    }
                }
                else if (!IsPing) {
                    this.Queue.push(Value);
                    if (!this.WebSocket && !this.AutoTimeout)
                        this.Create();
                }
            }
        };
        Socket.prototype.CreateBuffer = function () { return new SocketBuffer(); };
        Socket.DefaultParameter = { Url: '/', DataType: 'arraybuffer', AutoTimeout: 0, PingData: null, PingTimeout: 50 * 1000 };
        Socket.DefaultEvents = { OnOpen: null, OnClose: null, OnError: null, OnData: null, OnString: null };
        return Socket;
    }());
    AutoCSer.Socket = Socket;
})(AutoCSer || (AutoCSer = {}));
