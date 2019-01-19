using System;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.Http
{
    /// <summary>
    /// 下载文件类型
    /// </summary>
#pragma warning disable
    internal enum ContentType
    {
        [ContentType(ExtensionName = "*", Name = "application/octet-stream")]
        _,
        [ContentType(ExtensionName = "323", Name = "text/h323")]
        _323,
        [ContentType(ExtensionName = "907", Name = "drawing/907")]
        _907,
        [ContentType(ExtensionName = "acp", Name = "audio/x-mei-aac")]
        acp,
        [ContentType(ExtensionName = "ai", Name = "application/postscript")]
        ai,
        [ContentType(ExtensionName = "aif", Name = "audio/aiff")]
        aif,
        [ContentType(ExtensionName = "aifc", Name = "audio/aiff")]
        aifc,
        [ContentType(ExtensionName = "aiff", Name = "audio/aiff")]
        aiff,
        [ContentType(ExtensionName = "apk", Name = "application/vnd.android.package-archive")]
        apk,
        [ContentType(ExtensionName = "asa", Name = "text/asa")]
        asa,
        [ContentType(ExtensionName = "asf", Name = "video/x-ms-asf")]
        asf,
        [ContentType(ExtensionName = "asp", Name = "text/asp")]
        asp,
        [ContentType(ExtensionName = "asx", Name = "video/x-ms-asf")]
        asx,
        [ContentType(ExtensionName = "au", Name = "audio/basic")]
        au,
        [ContentType(ExtensionName = "avi", Name = "video/avi")]
        avi,
        [ContentType(ExtensionName = "awf", Name = "application/vnd.adobe.workflow")]
        awf,
        [ContentType(ExtensionName = "biz", Name = "text/xml")]
        biz,
        [ContentType(ExtensionName = "bmp", Name = "image/msbitmap")]
        bmp,
        [ContentType(ExtensionName = "cat", Name = "application/vnd.ms-pki.seccat")]
        cat,
        [ContentType(ExtensionName = "cdf", Name = "application/x-netcdf")]
        cdf,
        [ContentType(ExtensionName = "cer", Name = "application/x-x509-ca-cert")]
        cer,
        [ContentType(ExtensionName = "class", Name = "java/*")]
        _class,
        [ContentType(ExtensionName = "cml", Name = "text/xml")]
        cml,
        [ContentType(ExtensionName = "crl", Name = "application/pkix-crl")]
        crl,
        [ContentType(ExtensionName = "crt", Name = "application/x-x509-ca-cert")]
        crt,
        [ContentType(ExtensionName = "css", Name = "text/css")]
        css,
        [ContentType(ExtensionName = "cur", Name = "image/x-icon")]
        cur,
        [ContentType(ExtensionName = "dcd", Name = "text/xml")]
        dcd,
        [ContentType(ExtensionName = "der", Name = "application/x-x509-ca-cert")]
        der,
        [ContentType(ExtensionName = "dll", Name = "application/x-msdownload")]
        dll,
        [ContentType(ExtensionName = "doc", Name = "application/msword")]
        doc,
        [ContentType(ExtensionName = "dot", Name = "application/msword")]
        dot,
        [ContentType(ExtensionName = "dtd", Name = "text/xml")]
        dtd,
        [ContentType(ExtensionName = "edn", Name = "application/vnd.adobe.edn")]
        edn,
        [ContentType(ExtensionName = "eml", Name = "message/rfc822")]
        eml,
        [ContentType(ExtensionName = "ent", Name = "text/xml")]
        ent,
        [ContentType(ExtensionName = "eot", Name = "application/font-eot")]
        eot,
        [ContentType(ExtensionName = "eps", Name = "application/postscript")]
        eps,
        [ContentType(ExtensionName = "exe", Name = "application/x-msdownload")]
        exe,
        [ContentType(ExtensionName = "fax", Name = "image/fax")]
        fax,
        [ContentType(ExtensionName = "fdf", Name = "application/vnd.fdf")]
        fdf,
        [ContentType(ExtensionName = "fif", Name = "application/fractals")]
        fif,
        [ContentType(ExtensionName = "flv", Name = "video/x-flv")]
        flv,
        [ContentType(ExtensionName = "fo", Name = "text/xml")]
        fo,
        [ContentType(ExtensionName = "gif", Name = "image/gif")]
        gif,
        [ContentType(ExtensionName = "hpg", Name = "application/x-hpgl")]
        hpg,
        [ContentType(ExtensionName = "hqx", Name = "application/mac-binhex40")]
        hqx,
        [ContentType(ExtensionName = "hta", Name = "application/hta")]
        hta,
        [ContentType(ExtensionName = "htc", Name = "text/x-component")]
        htc,
        [ContentType(ExtensionName = "htm", Name = "text/html")]
        htm,
        [ContentType(ExtensionName = "html", Name = "text/html")]
        html,
        [ContentType(ExtensionName = "htt", Name = "text/webviewhtml")]
        htt,
        [ContentType(ExtensionName = "htx", Name = "text/html")]
        htx,
        [ContentType(ExtensionName = "ico", Name = "image/x-icon")]
        ico,
        [ContentType(ExtensionName = "iii", Name = "application/x-iphone")]
        iii,
        [ContentType(ExtensionName = "img", Name = "application/x-img")]
        img,
        [ContentType(ExtensionName = "ins", Name = "application/x-internet-signup")]
        ins,
        [ContentType(ExtensionName = "isp", Name = "application/x-internet-signup")]
        isp,
        [ContentType(ExtensionName = "java", Name = "java/*")]
        java,
        [ContentType(ExtensionName = "jfif", Name = "image/jpeg")]
        jfif,
        [ContentType(ExtensionName = "jpe", Name = "image/jpeg")]
        jpe,
        [ContentType(ExtensionName = "jpeg", Name = "image/jpeg")]
        jpeg,
        [ContentType(ExtensionName = "jpg", Name = "image/jpeg")]
        jpg,
        [ContentType(ExtensionName = "js", Name = "application/x-javascript")]
        js,
        [ContentType(ExtensionName = "jsp", Name = "text/html")]
        jsp,
        [ContentType(ExtensionName = "la1", Name = "audio/x-liquid-file")]
        la1,
        [ContentType(ExtensionName = "lar", Name = "application/x-laplayer-reg")]
        lar,
        [ContentType(ExtensionName = "latex", Name = "application/x-latex")]
        latex,
        [ContentType(ExtensionName = "lavs", Name = "audio/x-liquid-secure")]
        lavs,
        [ContentType(ExtensionName = "lmsff", Name = "audio/x-la-lms")]
        lmsff,
        [ContentType(ExtensionName = "ls", Name = "application/x-javascript")]
        ls,
        [ContentType(ExtensionName = "m1v", Name = "video/x-mpeg")]
        m1v,
        [ContentType(ExtensionName = "m2v", Name = "video/x-mpeg")]
        m2v,
        [ContentType(ExtensionName = "m3u", Name = "audio/mpegurl")]
        m3u,
        [ContentType(ExtensionName = "m4e", Name = "video/mpeg4")]
        m4e,
        [ContentType(ExtensionName = "man", Name = "application/x-troff-man")]
        man,
        [ContentType(ExtensionName = "math", Name = "text/xml")]
        math,
        [ContentType(ExtensionName = "mdb", Name = "application/msaccess")]
        mdb,
        [ContentType(ExtensionName = "mfp", Name = "application/x-shockwave-flash")]
        mfp,
        [ContentType(ExtensionName = "mht", Name = "message/rfc822")]
        mht,
        [ContentType(ExtensionName = "mhtml", Name = "message/rfc822")]
        mhtml,
        [ContentType(ExtensionName = "mid", Name = "audio/mid")]
        mid,
        [ContentType(ExtensionName = "midi", Name = "audio/mid")]
        midi,
        [ContentType(ExtensionName = "mml", Name = "text/xml")]
        mml,
        [ContentType(ExtensionName = "mnd", Name = "audio/x-musicnet-download")]
        mnd,
        [ContentType(ExtensionName = "mns", Name = "audio/x-musicnet-stream")]
        mns,
        [ContentType(ExtensionName = "mocha", Name = "application/x-javascript")]
        mocha,
        [ContentType(ExtensionName = "movie", Name = "video/x-sgi-movie")]
        movie,
        [ContentType(ExtensionName = "mp1", Name = "audio/mp1")]
        mp1,
        [ContentType(ExtensionName = "mp2", Name = "audio/mp2")]
        mp2,
        [ContentType(ExtensionName = "mp2v", Name = "video/mpeg")]
        mp2v,
        [ContentType(ExtensionName = "mp3", Name = "audio/mp3")]
        mp3,
        [ContentType(ExtensionName = "mp4", Name = "video/mp4")]//mpeg4
        mp4,
        [ContentType(ExtensionName = "mpa", Name = "video/x-mpg")]
        mpa,
        [ContentType(ExtensionName = "mpd", Name = "application/vnd.ms-project")]
        mpd,
        [ContentType(ExtensionName = "mpe", Name = "video/x-mpeg")]
        mpe,
        [ContentType(ExtensionName = "mpeg", Name = "video/mpg")]
        mpeg,
        [ContentType(ExtensionName = "mpg", Name = "video/mpg")]
        mpg,
        [ContentType(ExtensionName = "mpga", Name = "audio/rn-mpeg")]
        mpga,
        [ContentType(ExtensionName = "mpp", Name = "application/vnd.ms-project")]
        mpp,
        [ContentType(ExtensionName = "mps", Name = "video/x-mpeg")]
        mps,
        [ContentType(ExtensionName = "mpt", Name = "application/vnd.ms-project")]
        mpt,
        [ContentType(ExtensionName = "mpv", Name = "video/mpg")]
        mpv,
        [ContentType(ExtensionName = "mpv2", Name = "video/mpeg")]
        mpv2,
        [ContentType(ExtensionName = "mpw", Name = "application/vnd.ms-project")]
        mpw,
        [ContentType(ExtensionName = "mpx", Name = "application/vnd.ms-project")]
        mpx,
        [ContentType(ExtensionName = "mtx", Name = "text/xml")]
        mtx,
        [ContentType(ExtensionName = "mxp", Name = "application/x-mmxp")]
        mxp,
        [ContentType(ExtensionName = "net", Name = "image/pnetvue")]
        net,
        [ContentType(ExtensionName = "nws", Name = "message/rfc822")]
        nws,
        [ContentType(ExtensionName = "odc", Name = "text/x-ms-odc")]
        odc,
        [ContentType(ExtensionName = "otf", Name = "application/font-otf")]
        otf,
        [ContentType(ExtensionName = "p10", Name = "application/pkcs10")]
        p10,
        [ContentType(ExtensionName = "p12", Name = "application/x-pkcs12")]
        p12,
        [ContentType(ExtensionName = "p7b", Name = "application/x-pkcs7-certificates")]
        p7b,
        [ContentType(ExtensionName = "p7c", Name = "application/pkcs7-mime")]
        p7c,
        [ContentType(ExtensionName = "p7m", Name = "application/pkcs7-mime")]
        p7m,
        [ContentType(ExtensionName = "p7r", Name = "application/x-pkcs7-certreqresp")]
        p7r,
        [ContentType(ExtensionName = "p7s", Name = "application/pkcs7-signature")]
        p7s,
        [ContentType(ExtensionName = "pcx", Name = "image/x-pcx")]
        pcx,
        [ContentType(ExtensionName = "pdf", Name = "application/pdf")]
        pdf,
        [ContentType(ExtensionName = "pdx", Name = "application/vnd.adobe.pdx")]
        pdx,
        [ContentType(ExtensionName = "pfx", Name = "application/x-pkcs12")]
        pfx,
        [ContentType(ExtensionName = "pic", Name = "application/x-pic")]
        pic,
        [ContentType(ExtensionName = "pko", Name = "application/vnd.ms-pki.pko")]
        pko,
        [ContentType(ExtensionName = "pl", Name = "application/x-perl")]
        pl,
        [ContentType(ExtensionName = "plg", Name = "text/html")]
        plg,
        [ContentType(ExtensionName = "pls", Name = "audio/scpls")]
        pls,
        [ContentType(ExtensionName = "png", Name = "image/png")]
        png,
        [ContentType(ExtensionName = "pot", Name = "application/vnd.ms-powerpoint")]
        pot,
        [ContentType(ExtensionName = "ppa", Name = "application/vnd.ms-powerpoint")]
        ppa,
        [ContentType(ExtensionName = "pps", Name = "application/vnd.ms-powerpoint")]
        pps,
        [ContentType(ExtensionName = "ppt", Name = "application/vnd.ms-powerpoint")]
        ppt,
        [ContentType(ExtensionName = "prf", Name = "application/pics-rules")]
        prf,
        [ContentType(ExtensionName = "ps", Name = "application/postscript")]
        ps,
        [ContentType(ExtensionName = "pwz", Name = "application/vnd.ms-powerpoint")]
        pwz,
        [ContentType(ExtensionName = "r3t", Name = "text/vnd.rn-realtext3d")]
        r3t,
        [ContentType(ExtensionName = "ra", Name = "audio/vnd.rn-realaudio")]
        ra,
        [ContentType(ExtensionName = "ram", Name = "audio/x-pn-realaudio")]
        ram,
        [ContentType(ExtensionName = "rat", Name = "application/rat-file")]
        rat,
        [ContentType(ExtensionName = "rdf", Name = "text/xml")]
        rdf,
        [ContentType(ExtensionName = "rec", Name = "application/vnd.rn-recording")]
        rec,
        [ContentType(ExtensionName = "rjs", Name = "application/vnd.rn-realsystem-rjs")]
        rjs,
        [ContentType(ExtensionName = "rjt", Name = "application/vnd.rn-realsystem-rjt")]
        rjt,
        [ContentType(ExtensionName = "rm", Name = "application/vnd.rn-realmedia")]
        rm,
        [ContentType(ExtensionName = "rmf", Name = "application/vnd.adobe.rmf")]
        rmf,
        [ContentType(ExtensionName = "rmi", Name = "audio/mid")]
        rmi,
        [ContentType(ExtensionName = "rmj", Name = "application/vnd.rn-realsystem-rmj")]
        rmj,
        [ContentType(ExtensionName = "rmm", Name = "audio/x-pn-realaudio")]
        rmm,
        [ContentType(ExtensionName = "rmp", Name = "application/vnd.rn-rn_music_package")]
        rmp,
        [ContentType(ExtensionName = "rms", Name = "application/vnd.rn-realmedia-secure")]
        rms,
        [ContentType(ExtensionName = "rmvb", Name = "application/vnd.rn-realmedia-vbr")]
        rmvb,
        [ContentType(ExtensionName = "rmx", Name = "application/vnd.rn-realsystem-rmx")]
        rmx,
        [ContentType(ExtensionName = "rnx", Name = "application/vnd.rn-realplayer")]
        rnx,
        [ContentType(ExtensionName = "rp", Name = "image/vnd.rn-realpix")]
        rp,
        [ContentType(ExtensionName = "rpm", Name = "audio/x-pn-realaudio-plugin")]
        rpm,
        [ContentType(ExtensionName = "rsml", Name = "application/vnd.rn-rsml")]
        rsml,
        [ContentType(ExtensionName = "rt", Name = "text/vnd.rn-realtext")]
        rt,
        [ContentType(ExtensionName = "rtf", Name = "application/msword")]
        rtf,
        [ContentType(ExtensionName = "rv", Name = "video/vnd.rn-realvideo")]
        rv,
        [ContentType(ExtensionName = "sit", Name = "application/x-stuffit")]
        sit,
        [ContentType(ExtensionName = "smi", Name = "application/smil")]
        smi,
        [ContentType(ExtensionName = "smil", Name = "application/smil")]
        smil,
        [ContentType(ExtensionName = "snd", Name = "audio/basic")]
        snd,
        [ContentType(ExtensionName = "sol", Name = "text/plain")]
        sol,
        [ContentType(ExtensionName = "sor", Name = "text/plain")]
        sor,
        [ContentType(ExtensionName = "spc", Name = "application/x-pkcs7-certificates")]
        spc,
        [ContentType(ExtensionName = "spl", Name = "application/futuresplash")]
        spl,
        [ContentType(ExtensionName = "spp", Name = "text/xml")]
        spp,
        [ContentType(ExtensionName = "ssm", Name = "application/streamingmedia")]
        ssm,
        [ContentType(ExtensionName = "sst", Name = "application/vnd.ms-pki.certstore")]
        sst,
        [ContentType(ExtensionName = "stl", Name = "application/vnd.ms-pki.stl")]
        stl,
        [ContentType(ExtensionName = "stm", Name = "text/html")]
        stm,
        [ContentType(ExtensionName = "svg", Name = "text/xml")]
        svg,
        [ContentType(ExtensionName = "swf", Name = "application/x-shockwave-flash")]
        swf,
        [ContentType(ExtensionName = "tif", Name = "image/tiff")]
        tif,
        [ContentType(ExtensionName = "tiff", Name = "image/tiff")]
        tiff,
        [ContentType(ExtensionName = "tld", Name = "text/xml")]
        tld,
        [ContentType(ExtensionName = "torrent", Name = "application/x-bittorrent")]
        torrent,
        [ContentType(ExtensionName = "tsd", Name = "text/xml")]
        tsd,
        [ContentType(ExtensionName = "txt", Name = "text/plain")]
        txt,
        [ContentType(ExtensionName = "uin", Name = "application/x-icq")]
        uin,
        [ContentType(ExtensionName = "uls", Name = "text/iuls")]
        uls,
        [ContentType(ExtensionName = "vcf", Name = "text/x-vcard")]
        vcf,
        [ContentType(ExtensionName = "vdx", Name = "application/vnd.visio")]
        vdx,
        [ContentType(ExtensionName = "vml", Name = "text/xml")]
        vml,
        [ContentType(ExtensionName = "vpg", Name = "application/x-vpeg005")]
        vpg,
        [ContentType(ExtensionName = "vsd", Name = "application/vnd.visio")]
        vsd,
        [ContentType(ExtensionName = "vss", Name = "application/vnd.visio")]
        vss,
        [ContentType(ExtensionName = "vst", Name = "application/vnd.visio")]
        vst,
        [ContentType(ExtensionName = "vsw", Name = "application/vnd.visio")]
        vsw,
        [ContentType(ExtensionName = "vsx", Name = "application/vnd.visio")]
        vsx,
        [ContentType(ExtensionName = "vtx", Name = "application/vnd.visio")]
        vtx,
        [ContentType(ExtensionName = "vxml", Name = "text/xml")]
        vxml,
        [ContentType(ExtensionName = "wav", Name = "audio/wav")]
        wav,
        [ContentType(ExtensionName = "wax", Name = "audio/x-ms-wax")]
        wax,
        [ContentType(ExtensionName = "wbmp", Name = "image/vnd.wap.wbmp")]
        wbmp,
        [ContentType(ExtensionName = "wiz", Name = "application/msword")]
        wiz,
        [ContentType(ExtensionName = "wm", Name = "video/x-ms-wm")]
        wm,
        [ContentType(ExtensionName = "wma", Name = "audio/x-ms-wma")]
        wma,
        [ContentType(ExtensionName = "wmd", Name = "application/x-ms-wmd")]
        wmd,
        [ContentType(ExtensionName = "wml", Name = "text/vnd.wap.wml")]
        wml,
        [ContentType(ExtensionName = "wmv", Name = "video/x-ms-wmv")]
        wmv,
        [ContentType(ExtensionName = "wmx", Name = "video/x-ms-wmx")]
        wmx,
        [ContentType(ExtensionName = "wmz", Name = "application/x-ms-wmz")]
        wmz,
        [ContentType(ExtensionName = "woff", Name = "application/font-woff")]
        woff,
        [ContentType(ExtensionName = "wpl", Name = "application/vnd.ms-wpl")]
        wpl,
        [ContentType(ExtensionName = "wsc", Name = "text/scriptlet")]
        wsc,
        [ContentType(ExtensionName = "wsdl", Name = "text/xml")]
        wsdl,
        [ContentType(ExtensionName = "wvx", Name = "video/x-ms-wvx")]
        wvx,
        [ContentType(ExtensionName = "xdp", Name = "application/vnd.adobe.xdp")]
        xdp,
        [ContentType(ExtensionName = "xdr", Name = "text/xml")]
        xdr,
        [ContentType(ExtensionName = "xfd", Name = "application/vnd.adobe.xfd")]
        xfd,
        [ContentType(ExtensionName = "xfdf", Name = "application/vnd.adobe.xfdf")]
        xfdf,
        [ContentType(ExtensionName = "xhtml", Name = "text/html")]
        xhtml,
        [ContentType(ExtensionName = "xls", Name = "application/vnd.ms-excel")]
        xls,
        [ContentType(ExtensionName = "xml", Name = "text/xml")]
        xml,
        [ContentType(ExtensionName = "xpl", Name = "audio/scpls")]
        xpl,
        [ContentType(ExtensionName = "xq", Name = "text/xml")]
        xq,
        [ContentType(ExtensionName = "xql", Name = "text/xml")]
        xql,
        [ContentType(ExtensionName = "xquery", Name = "text/xml")]
        xquery,
        [ContentType(ExtensionName = "xsd", Name = "text/xml")]
        xsd,
        [ContentType(ExtensionName = "xsl", Name = "text/xml")]
        xsl,
        [ContentType(ExtensionName = "xslt", Name = "text/xml")]
        xslt,
    }
#pragma warning restore
    /// <summary>
    /// 下载文件类型属性
    /// </summary>
    internal sealed class ContentTypeAttribute : Attribute
    {
        /// <summary>
        /// 文件扩展名
        /// </summary>
        public string ExtensionName;
        /// <summary>
        /// 下载文件类型名称
        /// </summary>
        public string Name;

        /// <summary>
        /// 扩展名关联下载文件类型
        /// </summary>
        private static readonly AutoCSer.StateSearcher.AsciiSearcher<byte[]> contentTypes;
        /// <summary>
        /// 未知扩展名关联下载文件类型
        /// </summary>
        private static readonly byte[] unknownContentType;
        /// <summary>
        /// 默认内容类型头部
        /// </summary>
        internal static readonly byte[] Html = ("text/html; charset=" + AutoCSer.Config.Pub.Default.Encoding.WebName).getBytes();
        /// <summary>
        /// 默认内容类型头部
        /// </summary>
        internal static readonly byte[] Js = ("application/x-javascript; charset=" + AutoCSer.Config.Pub.Default.Encoding.WebName).getBytes();
        /// <summary>
        /// 内容类型头部
        /// </summary>
        internal static readonly byte[] Mp3;
        /// <summary>
        /// 内容类型头部
        /// </summary>
        internal static readonly byte[] Mp4;
        /// <summary>
        /// 内容类型头部
        /// </summary>
        internal static readonly byte[] Rmvb;
        /// <summary>
        /// 内容类型头部
        /// </summary>
        internal static readonly byte[] Doc;
        /// <summary>
        /// 内容类型头部
        /// </summary>
        internal static readonly byte[] Woff;
        /// <summary>
        /// 内容类型头部
        /// </summary>
        internal static readonly byte[] Gif;
        /// <summary>
        /// 内容类型头部
        /// </summary>
        internal static readonly byte[] Swf;
        /// <summary>
        /// 内容类型头部
        /// </summary>
        internal static readonly byte[] Pdf;
        /// <summary>
        /// 内容类型头部
        /// </summary>
        internal static readonly byte[] Otf;
        /// <summary>
        /// 内容类型头部
        /// </summary>
        internal static readonly byte[] Jpeg;
        /// <summary>
        /// 内容类型头部
        /// </summary>
        internal static readonly byte[] Jpg;
        /// <summary>
        /// 内容类型头部
        /// </summary>
        internal static readonly byte[] Png;
        /// <summary>
        /// 内容类型头部
        /// </summary>
        internal static readonly byte[] Mpg;
        /// <summary>
        /// 内容类型头部
        /// </summary>
        internal static readonly byte[] Svg;
        /// <summary>
        /// 内容类型头部
        /// </summary>
        internal static readonly byte[] Avi;
        /// <summary>
        /// 内容类型头部
        /// </summary>
        internal static readonly byte[] Apk;
        /// <summary>
        /// 内容类型头部
        /// </summary>
        internal static readonly byte[] Xml;
        /// <summary>
        /// 内容类型头部
        /// </summary>
        internal static readonly byte[] Rm;
        /// <summary>
        /// 内容类型头部
        /// </summary>
        internal static readonly byte[] Ico;
        /// <summary>
        /// 内容类型头部
        /// </summary>
        internal static readonly byte[] Zip;
        /// <summary>
        /// 内容类型头部
        /// </summary>
        internal static readonly byte[] Bmp;
        /// <summary>
        /// 内容类型头部
        /// </summary>
        internal static readonly byte[] Rar;
        /// <summary>
        /// 内容类型头部
        /// </summary>
        internal static readonly byte[] Cur;
        /// <summary>
        /// 内容类型头部
        /// </summary>
        internal static readonly byte[] Css;
        /// <summary>
        /// 内容类型头部
        /// </summary>
        internal static readonly byte[] Xls;
        /// <summary>
        /// 内容类型头部
        /// </summary>
        internal static readonly byte[] Txt;
        /// <summary>
        /// 内容类型头部
        /// </summary>
        internal static readonly byte[] Eot;
        /// <summary>
        /// 内容类型头部
        /// </summary>
        internal static readonly byte[] Wav;
        /// <summary>
        /// 内容类型头部
        /// </summary>
        internal static readonly byte[] Wmv;
        /// <summary>
        /// 内容类型头部
        /// </summary>
        internal static readonly byte[] Docx;
        /// <summary>
        /// 内容类型头部
        /// </summary>
        internal static readonly byte[] Xlsx;
        /// <summary>
        /// 内容类型头部
        /// </summary>
        internal static readonly byte[] _7z;
        /// <summary>
        /// 获取扩展名关联下载文件类型
        /// </summary>
        /// <param name="extensionName">扩展名</param>
        /// <returns>扩展名关联下载文件类型</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static byte[] Get(string extensionName)
        {
            return contentTypes.Get(extensionName, unknownContentType);
        }
        /// <summary>
        /// 获取输出内容类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static byte[] Get(ResponseContentType type)
        {
            switch (type)
            {
                case ResponseContentType.Html: return Html;
                case ResponseContentType.Js: return Js;
                case ResponseContentType.Mp3: return Mp3;
                case ResponseContentType.Mp4: return Mp4;
                case ResponseContentType.Rmvb: return Rmvb;
                case ResponseContentType.Doc: return Doc;
                case ResponseContentType.Woff: return Woff;
                case ResponseContentType.Gif: return Gif;
                case ResponseContentType.Swf: return Swf;
                case ResponseContentType.Pdf: return Pdf;
                case ResponseContentType.Otf: return Otf;
                case ResponseContentType.Jpeg: return Jpeg;
                case ResponseContentType.Jpg: return Jpg;
                case ResponseContentType.Png: return Png;
                case ResponseContentType.Mpg: return Mpg;
                case ResponseContentType.Svg: return Svg;
                case ResponseContentType.Avi: return Avi;
                case ResponseContentType.Apk: return Apk;
                case ResponseContentType.Xml: return Xml;
                case ResponseContentType.Rm: return Rm;
                case ResponseContentType.Ico: return Ico;
                case ResponseContentType.Zip: return Zip;
                case ResponseContentType.Bmp: return Bmp;
                case ResponseContentType.Rar: return Rar;
                case ResponseContentType.Cur: return Cur;
                case ResponseContentType.Css: return Css;
                case ResponseContentType.Xls: return Xls;
                case ResponseContentType.Txt: return Txt;
                case ResponseContentType.Eot: return Eot;
                case ResponseContentType.Wav: return Wav;
                case ResponseContentType.Wmv: return Wmv;
                case ResponseContentType.Docx: return Docx;
                case ResponseContentType.Xlsx: return Xlsx;
                case ResponseContentType._7z: return _7z;
            }
            return null;
        }
        static ContentTypeAttribute()
        {
            ContentTypeAttribute[] types = System.Enum.GetValues(typeof(ContentType))
                .toArray<ContentType>().getArray(value => AutoCSer.EnumAttribute<ContentType, ContentTypeAttribute>.Array((int)value));
            contentTypes = new AutoCSer.StateSearcher.AsciiSearcher<byte[]>(types.getArray(value => value.ExtensionName), types.getArray(value => value.Name.getBytes()), true);
            unknownContentType = contentTypes.Get("*");
            Mp3 = contentTypes.Get("mp3", unknownContentType);
            Mp4 = contentTypes.Get("mp4", unknownContentType);
            Rmvb = contentTypes.Get("rmvb", unknownContentType);
            Doc = contentTypes.Get("doc", unknownContentType);
            Woff = contentTypes.Get("woff", unknownContentType);
            Gif = contentTypes.Get("gif", unknownContentType);
            Swf = contentTypes.Get("swf", unknownContentType);
            Pdf = contentTypes.Get("pdf", unknownContentType);
            Otf = contentTypes.Get("otf", unknownContentType);
            Jpeg = contentTypes.Get("jpeg", unknownContentType);
            Jpg = contentTypes.Get("jpg", unknownContentType);
            Png = contentTypes.Get("png", unknownContentType);
            Mpg = contentTypes.Get("mpg", unknownContentType);
            Svg = contentTypes.Get("svg", unknownContentType);
            Avi = contentTypes.Get("avi", unknownContentType);
            Apk = contentTypes.Get("apk", unknownContentType);
            Xml = contentTypes.Get("xml", unknownContentType);
            Rm = contentTypes.Get("rm", unknownContentType);
            Ico = contentTypes.Get("ico", unknownContentType);
            Zip = contentTypes.Get("zip", unknownContentType);
            Bmp = contentTypes.Get("bmp", unknownContentType);
            Rar = contentTypes.Get("rar", unknownContentType);
            Cur = contentTypes.Get("cur", unknownContentType);
            Css = contentTypes.Get("css", unknownContentType);
            Xls = contentTypes.Get("xls", unknownContentType);
            Txt = contentTypes.Get("txt", unknownContentType);
            Eot = contentTypes.Get("eot", unknownContentType);
            Wav = contentTypes.Get("wav", unknownContentType);
            Wmv = contentTypes.Get("wmv", unknownContentType);
            Docx = contentTypes.Get("docx", unknownContentType);
            Xlsx = contentTypes.Get("xlsx", unknownContentType);
            _7z = contentTypes.Get("7z", unknownContentType);
        }
    }
}
