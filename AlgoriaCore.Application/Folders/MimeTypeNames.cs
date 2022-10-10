using System;

namespace AlgoriaCore.Application.Folders
{
    /* Copiado de:
     * http://stackoverflow.com/questions/10362140/asp-mvc-are-there-any-constants-for-the-default-content-types */

    /// <summary>
    /// Tipos MIME comunes
    /// </summary>
    public static class MimeTypeNames
    {
        ///<summary>
        ///Usado para definir la codificación necesaria para archivos que contienen código fuente JavaScript. el tipo MIME alternativo para este tipo de archivo es text/javascript.
        ///</summary>
        public const string ApplicationXJavascript = "application/x-javascript";

        ///<summary>
        ///Audio 24bit Linear PCM a 8-48kHz, 1-N canales; Definido en RFC 3190
        ///</summary>
        public const string AudioL24 = "audio/L24";

        ///<summary>Archivos Adobe Flash por ejemplo con la extensión .swf</summary>
        public const string ApplicationXShockwaveFlash = "application/x-shockwave-flash";

        ///<summary>
        ///Datos binarios arbitrarios. [5] Generalmente hablando este tipo identifica archivos que no estan asociados con una aplicación específica.
        ///Contrario a las suposiciones anteriores de paquetes de software como Apache, este no es un tipo que deba aplicarse a archivos desconocidos.
        ///En tal caso, un servidor o aplicación no debe indicar un tipo de contenido, ya que puede ser incorrecto, sino que debe omitir el tipo para permitir que el destinatario adivine el tipo.[6]
        ///</summary>
        public const string ApplicationOctetStream = "application/octet-stream";

        ///<summary>Atom feeds</summary>
        public const string ApplicationAtomXml = "application/atom+xml";

        ///<summary>Hojas de estilo en cascada; Definido en RFC 2318</summary>
        public const string TextCss = "text/css";

        ///<summary>Comandos; subtipo residente en navegadores Gecko como Firefox 3.5</summary>
        public const string TextCmd = "text/cmd";

        ///<summary>Valores separados por coma; Definido en RFC 4180</summary>
        public const string TextCsv = "text/csv";

        ///<summary>deb (formato de archivo), un formagto de paquete de software usado por el proyecto Debian</summary>
        public const string ApplicationXDeb = "application/x-deb";

        ///<summary>Definido en RFC 1847</summary>
        public const string MultipartEncrypted = "multipart/encrypted";

        ///<summary>Definido en RFC 1847</summary>
        public const string MultipartSigned = "multipart/signed";

        ///<summary>Definido en RFC 2616</summary>
        public const string MessageHttp = "message/http";

        ///<summary>Definido en RFC 4735</summary>
        public const string ModelExample = "model/example";

        ///<summary>Documento independiente de dispositivo en formato DVI</summary>
        public const string ApplicationXDvi = "application/x-dvi";

        ///<summary>Archivos DTD; Definido por RFC 3023</summary>
        public const string ApplicationXmlDtd = "application/xml-dtd";

        ///<summary>ECMAScript/JavaScript; Definido en RFC 4329 (equivalente a application/ecmascript pero con reglas de procesamiento más flexibles) No es aceptado en IE o anteriroes - text/javascript es aceptado pero definido como obsoleto en RFC 4329. El atributo "type" de la etiqueta <script> en HTML5 es opcional y en la practica la omisión del tipo de medio de los programas JavaScript es la solución más interoperable  ya que todos los navegadores siempre ha asumido el valor predeterminado incluso antes HTML5.</summary>
        public const string ApplicationJavascript = "application/javascript";

        ///<summary>ECMAScript/JavaScript; Definido en RFC 4329 (equivalente a application/javascript pero con reglas de procesamiento más estrictas)</summary>
        public const string ApplicationEcmascript = "application/ecmascript";

        ///<summary>Datos EDI EDIFACT; Definido en RFC 1767</summary>
        public const string ApplicationEdifact = "application/EDIFACT";

        ///<summary>Datos EDI X12; Definido en RFC 1767</summary>
        public const string ApplicationEdiX12 = "application/EDI-X12";

        ///<summary>Email; Definido en RFC 2045 y RFC 2046</summary>
        public const string MessagePartial = "message/partial";

        ///<summary>Email; archivos EML, archivos MIME, archivos MHT, archivos MHTML; Definido en RFC 2045 y RFC 2046</summary>
        public const string MessageRfc822 = "message/rfc822";

        ///<summary>Languaje de marcado extensible; Definido en RFC 3023</summary>
        public const string TextXml = "text/xml";

        ///<summary>Video Flash (archivos FLV)</summary>
        public const string VideoXFlv = "video/x-flv";

        ///<summary>Imagen GIF; Definido en RFC 2045 y RFC 2046</summary>
        public const string ImageGif = "image/gif";

        ///<summary>Datos GoogleWebToolkit</summary>
        public const string TextXGwtRpc = "text/x-gwt-rpc";

        ///<summary>Gzip</summary>
        public const string ApplicationXGzip = "application/x-gzip";

        ///<summary>HTML; Definido en RFC 2854</summary>
        public const string TextHtml = "text/html";

        ///<summary>Imagen ICO; Registrado[9]</summary>
        public const string ImageVndMicrosoftIcon = "image/vnd.microsoft.icon";

        ///<summary>Archivos IGS, archivos IGES; Definido en RFC 2077</summary>
        public const string ModelIges = "model/iges";

        ///<summary>Notificación de disposición de mensaje instantáneo IMDN; Definido en RFC 5438</summary>
        public const string MessageImdnXml = "message/imdn+xml";

        ///<summary>Notación de Objetos JavaScript JSON; Definido en RFC 4627</summary>
        public const string ApplicationJson = "application/json";

        ///<summary>Parche de Notación de Objetos JavaScript (JSON); Definido en RFC 6902</summary>
        public const string ApplicationJsonPatch = "application/json-patch+json";

        ///<summary>JavaScript - Definido y obsoleto por RFC 4329 para desalentar su uso en favor de application/javascript. Sin embargo,text/javascript es permitido en HTML 4 y 5 y, a diferencia de application/javascript, tiene soporte multinavegador . El atributo "type" de la etiqueta <script> en HTML5 es opcional y no  hay necesidad de usarlo ya que todos los navegadores siempre han asumido el valor predeterminado correcto (aún HTML 4 donde era requerido por la especificación).</summary>
        [Obsolete("Actualmente se utiliza application/javascript")]
        public const string TextJavascript = "text/javascript";

        ///<summary>Imagen JPEG JFIF; Asociado con Internet Explorer; Listado en ms775147(v=vs.85) - JPEG Progresivo, iniciado antes del soporte global de navegadores para progresivos JPEGs (Microsoft and Firefox).</summary>
        public const string ImagePjpeg = "image/pjpeg";

        ///<summary>Imagen JPEG JFIF; Definido en RFC 2045 y RFC 2046</summary>
        public const string ImageJpeg = "image/jpeg";

        ///<summary>Datos de plantilla jQuery</summary>
        public const string TextXJqueryTmpl = "text/x-jquery-tmpl";

        ///<summary>Archivos KML (ej. para Google Earth)</summary>
        public const string ApplicationVndGoogleEarthKmlXml = "application/vnd.google-earth.kml+xml";

        ///<summary>Archivos LaTeX</summary>
        public const string ApplicationXLatex = "application/x-latex";

        ///<summary>Formato de medio abierto Matroska</summary>
        public const string VideoXMatroska = "video/x-matroska";

        ///<summary>Archivos Microsoft Excel 2007</summary>
        public const string ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        ///<summary>Archivos Microsoft Excel</summary>
        public const string ApplicationVndMsExcel = "application/vnd.ms-excel";

        ///<summary>Archivos Microsoft Powerpoint 2007</summary>
        public const string ApplicationVndOpenxmlformatsOfficedocumentPresentationmlPresentation = "application/vnd.openxmlformats-officedocument.presentationml.presentation";

        ///<summary>Archivos Microsoft Powerpoint</summary>
        public const string ApplicationVndMsPowerpoint = "application/vnd.ms-powerpoint";

        ///<summary>Archivos Microsoft Word 2007</summary>
        public const string ApplicationVndOpenxmlformatsOfficedocumentWordprocessingmlDocument = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";

        ///<summary>Archivos Microsoft Word [15]</summary>
        public const string ApplicationMsword = "application/msword";

        ///<summary>Correo electrónico MIME; Definido en RFC 2045 y RFC 2046</summary>
        public const string MultipartAlternative = "multipart/alternative";

        ///<summary>Correo electrónico MIME; Definido en RFC 2045 y RFC 2046</summary>
        public const string MultipartMixed = "multipart/mixed";

        ///<summary>Correo electrónico MIME; Definido en RFC 2387 y usado por MHTML (correo electrónico HTML)</summary>
        public const string MultipartRelated = "multipart/related";

        ///<summary>Formulario web MIME; Definido en RFC 2388</summary>
        public const string MultipartFormData = "multipart/form-data";

        ///<summary>Archivos Mozilla XUL</summary>
        public const string ApplicationVndMozillaXulXml = "application/vnd.mozilla.xul+xml";

        ///<summary>Audio MP3 y otros audios MPEG; Definido en RFC 3003</summary>
        public const string AudioMpeg = "audio/mpeg";

        ///<summary>Audio MP4</summary>
        public const string AudioMp4 = "audio/mp4";

        ///<summary>Video MP4; Definido en RFC 4337</summary>
        public const string VideoMp4 = "video/mp4";

        ///<summary>Video MPEG-1 con audio multiplexado; Definido en RFC 2045 y RFC 2046</summary>
        public const string VideoMpeg = "video/mpeg";

        ///<summary>Archivos MSH, archivos MESH; Definido en RFC 2077, archivos SILO</summary>
        public const string ModelMesh = "model/mesh";

        ///<summary>Audio mulaw a 8 kHz, 1 canal; Definido en RFC 2046</summary>
        public const string AudioBasic = "audio/basic";

        ///<summary>Ogg Theora o otros videos (con audio); Definido en RFC 5334</summary>
        public const string VideoOgg = "video/ogg";

        ///<summary>Ogg Vorbis, Speex, Flac y otro audio; Definido en RFC 5334</summary>
        public const string AudioOgg = "audio/ogg";

        ///<summary>Ogg, un formato de contenedor de flujo de bits multimedia; Definido en RFC 5334</summary>
        public const string ApplicationOgg = "application/ogg";

        ///<summary>OP</summary>
        public const string ApplicationXopXml = "application/xop+xml";

        ///<summary>Gráficos OpenDocument; Registrado[14]</summary>
        public const string ApplicationVndOasisOpendocumentGraphics = "application/vnd.oasis.opendocument.graphics";

        ///<summary>Presentación OpenDocument; Registrado[13]</summary>
        public const string ApplicationVndOasisOpendocumentPresentation = "application/vnd.oasis.opendocument.presentation";

        ///<summary>Hoja de cálculo OpenDocument; Registrado[12]</summary>
        public const string ApplicationVndOasisOpendocumentSpreadsheet = "application/vnd.oasis.opendocument.spreadsheet";

        ///<summary>Texto OpenDocument; Registrado[11]</summary>
        public const string ApplicationVndOasisOpendocumentText = "application/vnd.oasis.opendocument.text";

        ///<summary>Archivos p12</summary>
        public const string ApplicationXPkcs12 = "application/x-pkcs12";

        ///<summary>Archivos p7b y spc</summary>
        public const string ApplicationXPkcs7Certificates = "application/x-pkcs7-certificates";

        ///<summary>Archivos p7c</summary>
        public const string ApplicationXPkcs7Mime = "application/x-pkcs7-mime";

        ///<summary>Archivos p7r</summary>
        public const string ApplicationXPkcs7Certreqresp = "application/x-pkcs7-certreqresp";

        ///<summary>Archivos p7s</summary>
        public const string ApplicationXPkcs7Signature = "application/x-pkcs7-signature";

        ///<summary>Formato de documento portable, PDF ha estado en uso como documento de intercambio en Internet desde 1993; Definido en RFC 3778</summary>
        public const string ApplicationPdf = "application/pdf";

        ///<summary>Grpaficos de red portables; Registrado,[8] Definido en RFC 2083</summary>
        public const string ImagePng = "image/png";

        ///<summary>PostScript; Definido en RFC 2046</summary>
        public const string ApplicationPostscript = "application/postscript";

        ///<summary>Video QuickTime; Registrado[10]</summary>
        public const string VideoQuicktime = "video/quicktime";

        ///<summary>Archivos RAR</summary>
        public const string ApplicationXRarCompressed = "application/x-rar-compressed";

        ///<summary>RealAudio; Documentado en la respuesta de soporte para clientes de RealPlayer  2559</summary>
        public const string AudioVndRnRealaudio = "audio/vnd.rn-realaudio";

        ///<summary>Resource Description Framework; Definido por RFC 3870</summary>
        public const string ApplicationRdfXml = "application/rdf+xml";

        ///<summary>RSS feeds</summary>
        public const string ApplicationRssXml = "application/rss+xml";

        ///<summary>SOAP; Definido por RFC 3902</summary>
        public const string ApplicationSoapXml = "application/soap+xml";

        ///<summary>Archivos StuffIt</summary>
        public const string ApplicationXStuffit = "application/x-stuffit";

        ///<summary>Imagen de vectores SVG; Definido en SVG Tiny 1.2 Especificación Apéndice M</summary>
        public const string ImageSvgXml = "image/svg+xml";

        ///<summary>Formato de archivo de imagen de etiqueta (solo para Baseline TIFF); Definido en RFC 3302</summary>
        public const string ImageTiff = "image/tiff";

        ///<summary>Archivos Tarball</summary>
        public const string ApplicationXTar = "application/x-tar";

        ///<summary>Datos textuales; Definido en RFC 2046 y RFC 3676</summary>
        public const string TextPlain = "text/plain";

        ///<summary>Tipo de fuente MIME no registrada TrueType, pero es la más usada</summary>
        public const string ApplicationXFontTtf = "application/x-font-ttf";

        ///<summary>vCard (información de conracto); Definido en RFC 6350</summary>
        public const string TextVcard = "text/vcard";

        ///<summary>Audio codificado Vorbis; Definido en RFC 5215</summary>
        public const string AudioVorbis = "audio/vorbis";

        ///<summary>Audio WAV; Definido en RFC 2361</summary>
        public const string AudioVndWave = "audio/vnd.wave";

        ///<summary>Web Open Font Format; (candidate recommendation; use application/x-font-woff until standard is official)</summary>
        public const string ApplicationFontWoff = "application/font-woff";

        ///<summary>Formato de medio abierto basado en WebM Matroska</summary>
        public const string VideoWebm = "video/webm";

        ///<summary>Formato de medio abierto WebM</summary>
        public const string AudioWebm = "audio/webm";

        ///<summary>Windows Media Audio Redirector; Documentado en la página de ayuda de Microsoft</summary>
        public const string AudioXMsWax = "audio/x-ms-wax";

        ///<summary>Windows Media Audio; Documentado en Microsoft KB 288102</summary>
        public const string AudioXMsWma = "audio/x-ms-wma";

        ///<summary>Windows Media Video; Documentado en Microsoft KB 288102</summary>
        public const string VideoXMsWmv = "video/x-ms-wmv";

        ///<summary>Archivos WRL, archivos VRML; Definido en RFC 2077</summary>
        public const string ModelVrml = "model/vrml";

        ///<summary>Estándar X3D ISO para representación de gráficos de computadora en 3D, archivos X3D XML</summary>
        public const string ModelX3DXml = "model/x3d+xml";

        ///<summary>Estándar X3D ISO para representación de gráficos de computadora en 3D, archivos binarios X3DB</summary>
        public const string ModelX3DBinary = "model/x3d+binary";

        ///<summary>Estándar X3D ISO para representación de gráficos de computadora en 3D, archivos X3DV VRML</summary>
        public const string ModelX3DVrml = "model/x3d+vrml";

        ///<summary>XHTML; Definido por RFC 3236</summary>
        public const string ApplicationXhtmlXml = "application/xhtml+xml";

        ///<summary>Archivos ZIP; Registrado[7]</summary>
        public const string ApplicationZip = "application/zip";
    }
}
