--
-- PostgreSQL database dump
--

-- Dumped from database version 15.1
-- Dumped by pg_dump version 15.1

-- Started on 2023-01-24 13:08:44

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;


SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- TOC entry 287 (class 1259 OID 19573)
-- Name: AuditLog; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."AuditLog" (
    "Id" bigint NOT NULL,
    "TenantId" integer,
    "UserId" bigint,
    "ImpersonalizerUserId" bigint,
    "ServiceName" character varying(250),
    "MethodName" character varying(250),
    "Parameters" character varying,
    "ExecutionDatetime" timestamp without time zone,
    "ExecutionDuration" integer,
    "ClientIpAddress" character varying(64),
    "ClientName" character varying(128),
    "BroserInfo" character varying(250),
    "Exception" character varying,
    "CustomData" character varying,
    "Severity" smallint
);


ALTER TABLE public."AuditLog" OWNER TO postgres;

--
-- TOC entry 286 (class 1259 OID 18120)
-- Name: BinaryObjects; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."BinaryObjects" (
    "Id" uuid NOT NULL,
    "TenantId" integer,
    "Bytes" bytea,
    "FileName" character varying(250)
);


ALTER TABLE public."BinaryObjects" OWNER TO postgres;

--
-- TOC entry 221 (class 1259 OID 17562)
-- Name: ChangeLog; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."ChangeLog" (
    "Id" bigint NOT NULL,
    "UserId" bigint,
    "TenantId" integer,
    "table" character varying(30),
    key character varying(12),
    datetime timestamp without time zone
);


ALTER TABLE public."ChangeLog" OWNER TO postgres;

--
-- TOC entry 283 (class 1259 OID 18092)
-- Name: ChangeLogDetail; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."ChangeLogDetail" (
    "Id" bigint NOT NULL,
    changelog bigint,
    data character varying
);


ALTER TABLE public."ChangeLogDetail" OWNER TO postgres;

--
-- TOC entry 282 (class 1259 OID 18091)
-- Name: ChangeLogDetail_Id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public."ChangeLogDetail_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public."ChangeLogDetail_Id_seq" OWNER TO postgres;

--
-- TOC entry 3801 (class 0 OID 0)
-- Dependencies: 282
-- Name: ChangeLogDetail_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public."ChangeLogDetail_Id_seq" OWNED BY public."ChangeLogDetail"."Id";


--
-- TOC entry 220 (class 1259 OID 17561)
-- Name: ChangeLog_Id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public."ChangeLog_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public."ChangeLog_Id_seq" OWNER TO postgres;

--
-- TOC entry 3802 (class 0 OID 0)
-- Dependencies: 220
-- Name: ChangeLog_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public."ChangeLog_Id_seq" OWNED BY public."ChangeLog"."Id";


--
-- TOC entry 223 (class 1259 OID 17581)
-- Name: ChatMessage; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."ChatMessage" (
    "Id" bigint NOT NULL,
    "TenantId" integer,
    "UserId" bigint NOT NULL,
    "FriendTenantId" integer,
    "FriendUserId" bigint NOT NULL,
    "CreationTime" timestamp without time zone,
    "Message" character varying,
    "State" smallint,
    "Side" smallint
);


ALTER TABLE public."ChatMessage" OWNER TO postgres;

--
-- TOC entry 222 (class 1259 OID 17580)
-- Name: ChatMessage_Id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public."ChatMessage_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public."ChatMessage_Id_seq" OWNER TO postgres;

--
-- TOC entry 3803 (class 0 OID 0)
-- Dependencies: 222
-- Name: ChatMessage_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public."ChatMessage_Id_seq" OWNED BY public."ChatMessage"."Id";


--
-- TOC entry 225 (class 1259 OID 17612)
-- Name: ChatRoom; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."ChatRoom" (
    "Id" bigint NOT NULL,
    "TenantId" integer,
    "UserCreator" bigint,
    "ChatRoomId" character varying(50) NOT NULL,
    "Name" character varying(50) NOT NULL,
    "CreationTime" timestamp without time zone,
    "Description" character varying(250)
);


ALTER TABLE public."ChatRoom" OWNER TO postgres;

--
-- TOC entry 227 (class 1259 OID 17635)
-- Name: ChatRoomChat; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."ChatRoomChat" (
    "Id" bigint NOT NULL,
    "TenantId" integer,
    "ChatRoom" bigint NOT NULL,
    "User" bigint,
    "CreationTime" timestamp without time zone,
    "Comment" character varying
);


ALTER TABLE public."ChatRoomChat" OWNER TO postgres;

--
-- TOC entry 229 (class 1259 OID 17660)
-- Name: ChatRoomChatFile; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."ChatRoomChatFile" (
    "Id" bigint NOT NULL,
    "ChatRoomChat" bigint NOT NULL,
    "FileName" character varying(250),
    "FileExtension" character varying(15),
    "File" uuid
);


ALTER TABLE public."ChatRoomChatFile" OWNER TO postgres;

--
-- TOC entry 228 (class 1259 OID 17659)
-- Name: ChatRoomChatFile_Id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public."ChatRoomChatFile_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public."ChatRoomChatFile_Id_seq" OWNER TO postgres;

--
-- TOC entry 3804 (class 0 OID 0)
-- Dependencies: 228
-- Name: ChatRoomChatFile_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public."ChatRoomChatFile_Id_seq" OWNED BY public."ChatRoomChatFile"."Id";


--
-- TOC entry 231 (class 1259 OID 17672)
-- Name: ChatRoomChatUserTagged; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."ChatRoomChatUserTagged" (
    "Id" bigint NOT NULL,
    "ChatRoomChat" bigint NOT NULL,
    "UserTagged" bigint NOT NULL
);


ALTER TABLE public."ChatRoomChatUserTagged" OWNER TO postgres;

--
-- TOC entry 230 (class 1259 OID 17671)
-- Name: ChatRoomChatUserTagged_Id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public."ChatRoomChatUserTagged_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public."ChatRoomChatUserTagged_Id_seq" OWNER TO postgres;

--
-- TOC entry 3805 (class 0 OID 0)
-- Dependencies: 230
-- Name: ChatRoomChatUserTagged_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public."ChatRoomChatUserTagged_Id_seq" OWNED BY public."ChatRoomChatUserTagged"."Id";


--
-- TOC entry 226 (class 1259 OID 17634)
-- Name: ChatRoomChat_Id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public."ChatRoomChat_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public."ChatRoomChat_Id_seq" OWNER TO postgres;

--
-- TOC entry 3806 (class 0 OID 0)
-- Dependencies: 226
-- Name: ChatRoomChat_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public."ChatRoomChat_Id_seq" OWNED BY public."ChatRoomChat"."Id";


--
-- TOC entry 224 (class 1259 OID 17611)
-- Name: ChatRoom_Id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public."ChatRoom_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public."ChatRoom_Id_seq" OWNER TO postgres;

--
-- TOC entry 3807 (class 0 OID 0)
-- Dependencies: 224
-- Name: ChatRoom_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public."ChatRoom_Id_seq" OWNED BY public."ChatRoom"."Id";


--
-- TOC entry 233 (class 1259 OID 17689)
-- Name: Friendship; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Friendship" (
    "Id" bigint NOT NULL,
    "TenantId" integer,
    "UserId" bigint NOT NULL,
    "FriendTenantId" integer,
    "FriendUserId" bigint NOT NULL,
    "CreationTime" timestamp without time zone,
    "FriendNickname" character varying(50),
    "State" smallint
);


ALTER TABLE public."Friendship" OWNER TO postgres;

--
-- TOC entry 232 (class 1259 OID 17688)
-- Name: Friendship_Id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public."Friendship_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public."Friendship_Id_seq" OWNER TO postgres;

--
-- TOC entry 3808 (class 0 OID 0)
-- Dependencies: 232
-- Name: Friendship_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public."Friendship_Id_seq" OWNED BY public."Friendship"."Id";


--
-- TOC entry 235 (class 1259 OID 17718)
-- Name: Language; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Language" (
    "Id" integer NOT NULL,
    "TenantId" integer,
    "Name" character varying(10),
    "DisplayName" character varying(100),
    "IsActive" boolean NOT NULL
);


ALTER TABLE public."Language" OWNER TO postgres;

--
-- TOC entry 239 (class 1259 OID 17750)
-- Name: LanguageText; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."LanguageText" (
    "Id" bigint NOT NULL,
    "LanguageId" integer,
    "Key" character varying(100),
    "Value" character varying
);


ALTER TABLE public."LanguageText" OWNER TO postgres;

--
-- TOC entry 238 (class 1259 OID 17749)
-- Name: LanguageText_Id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public."LanguageText_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public."LanguageText_Id_seq" OWNER TO postgres;

--
-- TOC entry 3809 (class 0 OID 0)
-- Dependencies: 238
-- Name: LanguageText_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public."LanguageText_Id_seq" OWNED BY public."LanguageText"."Id";


--
-- TOC entry 234 (class 1259 OID 17717)
-- Name: Language_Id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public."Language_Id_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public."Language_Id_seq" OWNER TO postgres;

--
-- TOC entry 3810 (class 0 OID 0)
-- Dependencies: 234
-- Name: Language_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public."Language_Id_seq" OWNED BY public."Language"."Id";


--
-- TOC entry 249 (class 1259 OID 17834)
-- Name: MailServiceMail; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."MailServiceMail" (
    "Id" bigint NOT NULL,
    "TenantId" integer NOT NULL,
    "MailServiceRequest" bigint NOT NULL,
    "IsLocalConfig" boolean NOT NULL,
    "Sendto" character varying(1000),
    "CopyTo" character varying(1000),
    "BlindCopyTo" character varying(1000),
    "Subject" character varying(500)
);


ALTER TABLE public."MailServiceMail" OWNER TO postgres;

--
-- TOC entry 257 (class 1259 OID 17896)
-- Name: MailServiceMailAttach; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."MailServiceMailAttach" (
    "Id" bigint NOT NULL,
    "MailServiceMailBody" bigint,
    "ContenType" character varying(50),
    "FileName" character varying(200),
    "BinaryFile" bytea
);


ALTER TABLE public."MailServiceMailAttach" OWNER TO postgres;

--
-- TOC entry 256 (class 1259 OID 17895)
-- Name: MailServiceMailAttach_Id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public."MailServiceMailAttach_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public."MailServiceMailAttach_Id_seq" OWNER TO postgres;

--
-- TOC entry 3811 (class 0 OID 0)
-- Dependencies: 256
-- Name: MailServiceMailAttach_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public."MailServiceMailAttach_Id_seq" OWNED BY public."MailServiceMailAttach"."Id";


--
-- TOC entry 255 (class 1259 OID 17880)
-- Name: MailServiceMailBody; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."MailServiceMailBody" (
    "Id" bigint NOT NULL,
    "MailServiceMail" bigint NOT NULL,
    "Body" character varying
);


ALTER TABLE public."MailServiceMailBody" OWNER TO postgres;

--
-- TOC entry 254 (class 1259 OID 17879)
-- Name: MailServiceMailBody_Id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public."MailServiceMailBody_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public."MailServiceMailBody_Id_seq" OWNER TO postgres;

--
-- TOC entry 3812 (class 0 OID 0)
-- Dependencies: 254
-- Name: MailServiceMailBody_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public."MailServiceMailBody_Id_seq" OWNED BY public."MailServiceMailBody"."Id";


--
-- TOC entry 251 (class 1259 OID 17848)
-- Name: MailServiceMailConfig; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."MailServiceMailConfig" (
    "Id" bigint NOT NULL,
    "MailServiceMail" bigint NOT NULL,
    "Sender" character varying(250),
    "SenderDisplay" character varying(250),
    "SMPTHost" character varying(100),
    "SMPTPort" smallint,
    "IsSSL" boolean NOT NULL,
    "UseDefaultCredential" boolean NOT NULL,
    "Domain" character varying(100),
    "MailUser" character varying(50),
    "MailPassword" character varying(100)
);


ALTER TABLE public."MailServiceMailConfig" OWNER TO postgres;

--
-- TOC entry 250 (class 1259 OID 17847)
-- Name: MailServiceMailConfig_Id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public."MailServiceMailConfig_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public."MailServiceMailConfig_Id_seq" OWNER TO postgres;

--
-- TOC entry 3813 (class 0 OID 0)
-- Dependencies: 250
-- Name: MailServiceMailConfig_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public."MailServiceMailConfig_Id_seq" OWNED BY public."MailServiceMailConfig"."Id";


--
-- TOC entry 253 (class 1259 OID 17864)
-- Name: MailServiceMailStatus; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."MailServiceMailStatus" (
    "Id" bigint NOT NULL,
    "MailServiceMail" bigint NOT NULL,
    "SentTime" timestamp without time zone,
    "Status" smallint NOT NULL,
    "Error" character varying
);


ALTER TABLE public."MailServiceMailStatus" OWNER TO postgres;

--
-- TOC entry 252 (class 1259 OID 17863)
-- Name: MailServiceMailStatus_Id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public."MailServiceMailStatus_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public."MailServiceMailStatus_Id_seq" OWNER TO postgres;

--
-- TOC entry 3814 (class 0 OID 0)
-- Dependencies: 252
-- Name: MailServiceMailStatus_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public."MailServiceMailStatus_Id_seq" OWNED BY public."MailServiceMailStatus"."Id";


--
-- TOC entry 248 (class 1259 OID 17833)
-- Name: MailServiceMail_Id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public."MailServiceMail_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public."MailServiceMail_Id_seq" OWNER TO postgres;

--
-- TOC entry 3815 (class 0 OID 0)
-- Dependencies: 248
-- Name: MailServiceMail_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public."MailServiceMail_Id_seq" OWNED BY public."MailServiceMail"."Id";


--
-- TOC entry 247 (class 1259 OID 17815)
-- Name: MailServiceRequest; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."MailServiceRequest" (
    "Id" bigint NOT NULL,
    "TenantId" integer NOT NULL,
    "UserCreator" bigint NOT NULL,
    "CreationTime" timestamp without time zone
);


ALTER TABLE public."MailServiceRequest" OWNER TO postgres;

--
-- TOC entry 246 (class 1259 OID 17814)
-- Name: MailServiceRequest_Id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public."MailServiceRequest_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public."MailServiceRequest_Id_seq" OWNER TO postgres;

--
-- TOC entry 3816 (class 0 OID 0)
-- Dependencies: 246
-- Name: MailServiceRequest_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public."MailServiceRequest_Id_seq" OWNED BY public."MailServiceRequest"."Id";


--
-- TOC entry 261 (class 1259 OID 17924)
-- Name: OrgUnit; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."OrgUnit" (
    "Id" bigint NOT NULL,
    "TenantId" integer,
    "ParentOU" bigint,
    "Name" character varying(100) NOT NULL,
    "Level" smallint NOT NULL,
    "IsDeleted" boolean NOT NULL
);


ALTER TABLE public."OrgUnit" OWNER TO postgres;

--
-- TOC entry 263 (class 1259 OID 17943)
-- Name: OrgUnitUser; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."OrgUnitUser" (
    "Id" bigint NOT NULL,
    "OrgUnit" bigint NOT NULL,
    "User" bigint NOT NULL
);


ALTER TABLE public."OrgUnitUser" OWNER TO postgres;

--
-- TOC entry 262 (class 1259 OID 17942)
-- Name: OrgUnitUser_Id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public."OrgUnitUser_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public."OrgUnitUser_Id_seq" OWNER TO postgres;

--
-- TOC entry 3817 (class 0 OID 0)
-- Dependencies: 262
-- Name: OrgUnitUser_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public."OrgUnitUser_Id_seq" OWNED BY public."OrgUnitUser"."Id";


--
-- TOC entry 260 (class 1259 OID 17923)
-- Name: OrgUnit_Id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public."OrgUnit_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public."OrgUnit_Id_seq" OWNER TO postgres;

--
-- TOC entry 3818 (class 0 OID 0)
-- Dependencies: 260
-- Name: OrgUnit_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public."OrgUnit_Id_seq" OWNED BY public."OrgUnit"."Id";


--
-- TOC entry 285 (class 1259 OID 18106)
-- Name: Permission; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Permission" (
    "Id" bigint NOT NULL,
    "Role" bigint,
    "Name" character varying(150),
    "IsGranted" boolean NOT NULL
);


ALTER TABLE public."Permission" OWNER TO postgres;

--
-- TOC entry 284 (class 1259 OID 18105)
-- Name: Permission_Id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public."Permission_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public."Permission_Id_seq" OWNER TO postgres;

--
-- TOC entry 3819 (class 0 OID 0)
-- Dependencies: 284
-- Name: Permission_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public."Permission_Id_seq" OWNED BY public."Permission"."Id";


--
-- TOC entry 265 (class 1259 OID 17960)
-- Name: Role; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Role" (
    "Id" bigint NOT NULL,
    "TenantId" integer,
    "Name" character varying(50),
    "DisplayName" character varying(100),
    "IsActive" boolean NOT NULL,
    "IsDeleted" boolean NOT NULL
);


ALTER TABLE public."Role" OWNER TO postgres;

--
-- TOC entry 264 (class 1259 OID 17959)
-- Name: Role_Id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public."Role_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public."Role_Id_seq" OWNER TO postgres;

--
-- TOC entry 3820 (class 0 OID 0)
-- Dependencies: 264
-- Name: Role_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public."Role_Id_seq" OWNED BY public."Role"."Id";


--
-- TOC entry 269 (class 1259 OID 17989)
-- Name: SampleDateData; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."SampleDateData" (
    "Id" bigint NOT NULL,
    "TenantId" integer,
    "Name" character varying(50),
    "DateTimeData" timestamp without time zone,
    "DateData" date,
    "TimeData" time without time zone
);


ALTER TABLE public."SampleDateData" OWNER TO postgres;

--
-- TOC entry 268 (class 1259 OID 17988)
-- Name: SampleDateData_Id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public."SampleDateData_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public."SampleDateData_Id_seq" OWNER TO postgres;

--
-- TOC entry 3821 (class 0 OID 0)
-- Dependencies: 268
-- Name: SampleDateData_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public."SampleDateData_Id_seq" OWNED BY public."SampleDateData"."Id";


--
-- TOC entry 271 (class 1259 OID 18001)
-- Name: Setting; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Setting" (
    "Id" bigint NOT NULL,
    "TenantId" integer,
    "UserId" bigint,
    "Name" character varying(100),
    value character varying
);


ALTER TABLE public."Setting" OWNER TO postgres;

--
-- TOC entry 273 (class 1259 OID 18021)
-- Name: SettingClient; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."SettingClient" (
    "Id" bigint NOT NULL,
    "TenantId" integer,
    "UserId" bigint,
    "ClientType" character varying(50),
    "Name" character varying(100),
    value character varying
);


ALTER TABLE public."SettingClient" OWNER TO postgres;

--
-- TOC entry 272 (class 1259 OID 18020)
-- Name: SettingClient_Id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public."SettingClient_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public."SettingClient_Id_seq" OWNER TO postgres;

--
-- TOC entry 3822 (class 0 OID 0)
-- Dependencies: 272
-- Name: SettingClient_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public."SettingClient_Id_seq" OWNED BY public."SettingClient"."Id";


--
-- TOC entry 270 (class 1259 OID 18000)
-- Name: Setting_Id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public."Setting_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public."Setting_Id_seq" OWNER TO postgres;

--
-- TOC entry 3823 (class 0 OID 0)
-- Dependencies: 270
-- Name: Setting_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public."Setting_Id_seq" OWNED BY public."Setting"."Id";


--
-- TOC entry 217 (class 1259 OID 17536)
-- Name: Tenant; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Tenant" (
    "Id" integer NOT NULL,
    "TenancyName" character varying(50),
    "Name" character varying(150),
    "CreationTime" timestamp without time zone,
    "IsActive" boolean NOT NULL,
    "IsDeleted" boolean NOT NULL
);


ALTER TABLE public."Tenant" OWNER TO postgres;

--
-- TOC entry 277 (class 1259 OID 18055)
-- Name: TenantRegistration; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."TenantRegistration" (
    "Id" integer NOT NULL,
    "TenancyName" character varying(50),
    "TenantName" character varying(150),
    "UserLogin" character varying(32),
    "Password" character varying(200),
    "Name" character varying(50),
    "Lastname" character varying(50),
    "SecondLastname" character varying(50),
    "EmailAddress" character varying(250),
    "ConfirmationCode" character varying(40)
);


ALTER TABLE public."TenantRegistration" OWNER TO postgres;

--
-- TOC entry 276 (class 1259 OID 18054)
-- Name: TenantRegistration_Id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public."TenantRegistration_Id_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public."TenantRegistration_Id_seq" OWNER TO postgres;

--
-- TOC entry 3824 (class 0 OID 0)
-- Dependencies: 276
-- Name: TenantRegistration_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public."TenantRegistration_Id_seq" OWNED BY public."TenantRegistration"."Id";


--
-- TOC entry 216 (class 1259 OID 17535)
-- Name: Tenant_Id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public."Tenant_Id_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public."Tenant_Id_seq" OWNER TO postgres;

--
-- TOC entry 3825 (class 0 OID 0)
-- Dependencies: 216
-- Name: Tenant_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public."Tenant_Id_seq" OWNED BY public."Tenant"."Id";


--
-- TOC entry 219 (class 1259 OID 17546)
-- Name: User; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."User" (
    "Id" bigint NOT NULL,
    "TenantId" integer,
    "UserLogin" character varying(32),
    "Password" character varying(200),
    "Name" character varying(50),
    "Lastname" character varying(50),
    "SecondLastname" character varying(50),
    "EmailAddress" character varying(250),
    "IsEmailConfirmed" boolean,
    "PhoneNumber" character varying(20),
    "IsPhoneConfirmed" boolean,
    "CreationTime" timestamp without time zone,
    "ChangePassword" boolean,
    "AccessFailedCount" integer,
    "LastLoginTime" timestamp without time zone,
    "ProfilePictureId" uuid,
    "UserLocked" boolean,
    "IsLockoutEnabled" boolean,
    "IsActive" boolean NOT NULL,
    "IsDeleted" boolean NOT NULL
);


ALTER TABLE public."User" OWNER TO postgres;

--
-- TOC entry 275 (class 1259 OID 18041)
-- Name: UserARN; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."UserARN" (
    "Id" bigint NOT NULL,
    "UserId" bigint,
    "ARNCode" character varying(200)
);


ALTER TABLE public."UserARN" OWNER TO postgres;

--
-- TOC entry 274 (class 1259 OID 18040)
-- Name: UserARN_Id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public."UserARN_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public."UserARN_Id_seq" OWNER TO postgres;

--
-- TOC entry 3826 (class 0 OID 0)
-- Dependencies: 274
-- Name: UserARN_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public."UserARN_Id_seq" OWNED BY public."UserARN"."Id";


--
-- TOC entry 279 (class 1259 OID 18066)
-- Name: UserPasswordHistory; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."UserPasswordHistory" (
    "Id" bigint NOT NULL,
    "UserId" bigint,
    "Password" character varying(200),
    "Date" timestamp without time zone
);


ALTER TABLE public."UserPasswordHistory" OWNER TO postgres;

--
-- TOC entry 278 (class 1259 OID 18065)
-- Name: UserPasswordHistory_Id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public."UserPasswordHistory_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public."UserPasswordHistory_Id_seq" OWNER TO postgres;

--
-- TOC entry 3827 (class 0 OID 0)
-- Dependencies: 278
-- Name: UserPasswordHistory_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public."UserPasswordHistory_Id_seq" OWNED BY public."UserPasswordHistory"."Id";


--
-- TOC entry 281 (class 1259 OID 18078)
-- Name: UserReset; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."UserReset" (
    "Id" bigint NOT NULL,
    "UserId" bigint,
    "ResetCode" character varying(40),
    "Validity" timestamp without time zone
);


ALTER TABLE public."UserReset" OWNER TO postgres;

--
-- TOC entry 280 (class 1259 OID 18077)
-- Name: UserReset_Id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public."UserReset_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public."UserReset_Id_seq" OWNER TO postgres;

--
-- TOC entry 3828 (class 0 OID 0)
-- Dependencies: 280
-- Name: UserReset_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public."UserReset_Id_seq" OWNED BY public."UserReset"."Id";


--
-- TOC entry 267 (class 1259 OID 17972)
-- Name: UserRole; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."UserRole" (
    "Id" bigint NOT NULL,
    "UserId" bigint,
    "RoleId" bigint
);


ALTER TABLE public."UserRole" OWNER TO postgres;

--
-- TOC entry 266 (class 1259 OID 17971)
-- Name: UserRole_Id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public."UserRole_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public."UserRole_Id_seq" OWNER TO postgres;

--
-- TOC entry 3829 (class 0 OID 0)
-- Dependencies: 266
-- Name: UserRole_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public."UserRole_Id_seq" OWNED BY public."UserRole"."Id";


--
-- TOC entry 218 (class 1259 OID 17545)
-- Name: User_Id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public."User_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public."User_Id_seq" OWNER TO postgres;

--
-- TOC entry 3830 (class 0 OID 0)
-- Dependencies: 218
-- Name: User_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public."User_Id_seq" OWNED BY public."User"."Id";


--
-- TOC entry 214 (class 1259 OID 17525)
-- Name: __EFMigrationsHistory; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL
);


ALTER TABLE public."__EFMigrationsHistory" OWNER TO postgres;

--
-- TOC entry 215 (class 1259 OID 17530)
-- Name: __RefactorLog; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."__RefactorLog" (
    "OperationKey" uuid NOT NULL
);


ALTER TABLE public."__RefactorLog" OWNER TO postgres;

--
-- TOC entry 288 (class 1259 OID 19578)
-- Name: auditlog_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.auditlog_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.auditlog_id_seq OWNER TO postgres;

--
-- TOC entry 3831 (class 0 OID 0)
-- Dependencies: 288
-- Name: auditlog_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.auditlog_id_seq OWNED BY public."AuditLog"."Id";


--
-- TOC entry 237 (class 1259 OID 17730)
-- Name: help; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.help (
    "Id" bigint NOT NULL,
    "TenantId" integer,
    "LanguageId" integer NOT NULL,
    "HelpKey" character varying(50) NOT NULL,
    "DisplayName" character varying(100),
    "IsActive" boolean NOT NULL
);


ALTER TABLE public.help OWNER TO postgres;

--
-- TOC entry 236 (class 1259 OID 17729)
-- Name: help_Id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public."help_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public."help_Id_seq" OWNER TO postgres;

--
-- TOC entry 3832 (class 0 OID 0)
-- Dependencies: 236
-- Name: help_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public."help_Id_seq" OWNED BY public.help."Id";


--
-- TOC entry 241 (class 1259 OID 17766)
-- Name: helptxt; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.helptxt (
    "Id" bigint NOT NULL,
    help bigint NOT NULL,
    body character varying
);


ALTER TABLE public.helptxt OWNER TO postgres;

--
-- TOC entry 240 (class 1259 OID 17765)
-- Name: helptxt_Id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public."helptxt_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public."helptxt_Id_seq" OWNER TO postgres;

--
-- TOC entry 3833 (class 0 OID 0)
-- Dependencies: 240
-- Name: helptxt_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public."helptxt_Id_seq" OWNED BY public.helptxt."Id";


--
-- TOC entry 243 (class 1259 OID 17782)
-- Name: mailgroup; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.mailgroup (
    "Id" bigint NOT NULL,
    "TenantId" integer,
    "DisplayName" character varying(100)
);


ALTER TABLE public.mailgroup OWNER TO postgres;

--
-- TOC entry 242 (class 1259 OID 17781)
-- Name: mailgroup_Id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public."mailgroup_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public."mailgroup_Id_seq" OWNER TO postgres;

--
-- TOC entry 3834 (class 0 OID 0)
-- Dependencies: 242
-- Name: mailgroup_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public."mailgroup_Id_seq" OWNED BY public.mailgroup."Id";


--
-- TOC entry 259 (class 1259 OID 17910)
-- Name: mailgrouptxt; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.mailgrouptxt (
    "Id" bigint NOT NULL,
    mailgroup bigint,
    type smallint,
    body character varying
);


ALTER TABLE public.mailgrouptxt OWNER TO postgres;

--
-- TOC entry 258 (class 1259 OID 17909)
-- Name: mailgrouptxt_Id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public."mailgrouptxt_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public."mailgrouptxt_Id_seq" OWNER TO postgres;

--
-- TOC entry 3835 (class 0 OID 0)
-- Dependencies: 258
-- Name: mailgrouptxt_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public."mailgrouptxt_Id_seq" OWNED BY public.mailgrouptxt."Id";


--
-- TOC entry 245 (class 1259 OID 17794)
-- Name: mailtemplate; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.mailtemplate (
    "Id" bigint NOT NULL,
    "TenantId" integer,
    mailgroup bigint,
    mailkey character varying(50),
    "DisplayName" character varying(100),
    "SendTo" character varying(1000),
    "CopyTo" character varying(1000),
    "BlindCopyTo" character varying(1000),
    "Subject" character varying(250),
    "Body" character varying,
    "IsActive" boolean NOT NULL
);


ALTER TABLE public.mailtemplate OWNER TO postgres;

--
-- TOC entry 244 (class 1259 OID 17793)
-- Name: mailtemplate_Id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public."mailtemplate_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public."mailtemplate_Id_seq" OWNER TO postgres;

--
-- TOC entry 3836 (class 0 OID 0)
-- Dependencies: 244
-- Name: mailtemplate_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public."mailtemplate_Id_seq" OWNED BY public.mailtemplate."Id";


--
-- TOC entry 3395 (class 2604 OID 19596)
-- Name: AuditLog Id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."AuditLog" ALTER COLUMN "Id" SET DEFAULT nextval('public.auditlog_id_seq'::regclass);


--
-- TOC entry 3362 (class 2604 OID 17565)
-- Name: ChangeLog Id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."ChangeLog" ALTER COLUMN "Id" SET DEFAULT nextval('public."ChangeLog_Id_seq"'::regclass);


--
-- TOC entry 3393 (class 2604 OID 18095)
-- Name: ChangeLogDetail Id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."ChangeLogDetail" ALTER COLUMN "Id" SET DEFAULT nextval('public."ChangeLogDetail_Id_seq"'::regclass);


--
-- TOC entry 3363 (class 2604 OID 17584)
-- Name: ChatMessage Id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."ChatMessage" ALTER COLUMN "Id" SET DEFAULT nextval('public."ChatMessage_Id_seq"'::regclass);


--
-- TOC entry 3364 (class 2604 OID 17615)
-- Name: ChatRoom Id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."ChatRoom" ALTER COLUMN "Id" SET DEFAULT nextval('public."ChatRoom_Id_seq"'::regclass);


--
-- TOC entry 3365 (class 2604 OID 17638)
-- Name: ChatRoomChat Id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."ChatRoomChat" ALTER COLUMN "Id" SET DEFAULT nextval('public."ChatRoomChat_Id_seq"'::regclass);


--
-- TOC entry 3366 (class 2604 OID 17663)
-- Name: ChatRoomChatFile Id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."ChatRoomChatFile" ALTER COLUMN "Id" SET DEFAULT nextval('public."ChatRoomChatFile_Id_seq"'::regclass);


--
-- TOC entry 3367 (class 2604 OID 17675)
-- Name: ChatRoomChatUserTagged Id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."ChatRoomChatUserTagged" ALTER COLUMN "Id" SET DEFAULT nextval('public."ChatRoomChatUserTagged_Id_seq"'::regclass);


--
-- TOC entry 3368 (class 2604 OID 17692)
-- Name: Friendship Id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Friendship" ALTER COLUMN "Id" SET DEFAULT nextval('public."Friendship_Id_seq"'::regclass);


--
-- TOC entry 3369 (class 2604 OID 17721)
-- Name: Language Id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Language" ALTER COLUMN "Id" SET DEFAULT nextval('public."Language_Id_seq"'::regclass);


--
-- TOC entry 3371 (class 2604 OID 17753)
-- Name: LanguageText Id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."LanguageText" ALTER COLUMN "Id" SET DEFAULT nextval('public."LanguageText_Id_seq"'::regclass);


--
-- TOC entry 3376 (class 2604 OID 17837)
-- Name: MailServiceMail Id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."MailServiceMail" ALTER COLUMN "Id" SET DEFAULT nextval('public."MailServiceMail_Id_seq"'::regclass);


--
-- TOC entry 3380 (class 2604 OID 17899)
-- Name: MailServiceMailAttach Id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."MailServiceMailAttach" ALTER COLUMN "Id" SET DEFAULT nextval('public."MailServiceMailAttach_Id_seq"'::regclass);


--
-- TOC entry 3379 (class 2604 OID 17883)
-- Name: MailServiceMailBody Id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."MailServiceMailBody" ALTER COLUMN "Id" SET DEFAULT nextval('public."MailServiceMailBody_Id_seq"'::regclass);


--
-- TOC entry 3377 (class 2604 OID 17851)
-- Name: MailServiceMailConfig Id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."MailServiceMailConfig" ALTER COLUMN "Id" SET DEFAULT nextval('public."MailServiceMailConfig_Id_seq"'::regclass);


--
-- TOC entry 3378 (class 2604 OID 17867)
-- Name: MailServiceMailStatus Id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."MailServiceMailStatus" ALTER COLUMN "Id" SET DEFAULT nextval('public."MailServiceMailStatus_Id_seq"'::regclass);


--
-- TOC entry 3375 (class 2604 OID 17818)
-- Name: MailServiceRequest Id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."MailServiceRequest" ALTER COLUMN "Id" SET DEFAULT nextval('public."MailServiceRequest_Id_seq"'::regclass);


--
-- TOC entry 3382 (class 2604 OID 17927)
-- Name: OrgUnit Id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."OrgUnit" ALTER COLUMN "Id" SET DEFAULT nextval('public."OrgUnit_Id_seq"'::regclass);


--
-- TOC entry 3383 (class 2604 OID 17946)
-- Name: OrgUnitUser Id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."OrgUnitUser" ALTER COLUMN "Id" SET DEFAULT nextval('public."OrgUnitUser_Id_seq"'::regclass);


--
-- TOC entry 3394 (class 2604 OID 18109)
-- Name: Permission Id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Permission" ALTER COLUMN "Id" SET DEFAULT nextval('public."Permission_Id_seq"'::regclass);


--
-- TOC entry 3384 (class 2604 OID 17963)
-- Name: Role Id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Role" ALTER COLUMN "Id" SET DEFAULT nextval('public."Role_Id_seq"'::regclass);


--
-- TOC entry 3386 (class 2604 OID 17992)
-- Name: SampleDateData Id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."SampleDateData" ALTER COLUMN "Id" SET DEFAULT nextval('public."SampleDateData_Id_seq"'::regclass);


--
-- TOC entry 3387 (class 2604 OID 18004)
-- Name: Setting Id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Setting" ALTER COLUMN "Id" SET DEFAULT nextval('public."Setting_Id_seq"'::regclass);


--
-- TOC entry 3388 (class 2604 OID 18024)
-- Name: SettingClient Id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."SettingClient" ALTER COLUMN "Id" SET DEFAULT nextval('public."SettingClient_Id_seq"'::regclass);


--
-- TOC entry 3360 (class 2604 OID 17539)
-- Name: Tenant Id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Tenant" ALTER COLUMN "Id" SET DEFAULT nextval('public."Tenant_Id_seq"'::regclass);


--
-- TOC entry 3390 (class 2604 OID 18058)
-- Name: TenantRegistration Id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."TenantRegistration" ALTER COLUMN "Id" SET DEFAULT nextval('public."TenantRegistration_Id_seq"'::regclass);


--
-- TOC entry 3361 (class 2604 OID 17549)
-- Name: User Id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."User" ALTER COLUMN "Id" SET DEFAULT nextval('public."User_Id_seq"'::regclass);


--
-- TOC entry 3389 (class 2604 OID 18044)
-- Name: UserARN Id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."UserARN" ALTER COLUMN "Id" SET DEFAULT nextval('public."UserARN_Id_seq"'::regclass);


--
-- TOC entry 3391 (class 2604 OID 18069)
-- Name: UserPasswordHistory Id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."UserPasswordHistory" ALTER COLUMN "Id" SET DEFAULT nextval('public."UserPasswordHistory_Id_seq"'::regclass);


--
-- TOC entry 3392 (class 2604 OID 18081)
-- Name: UserReset Id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."UserReset" ALTER COLUMN "Id" SET DEFAULT nextval('public."UserReset_Id_seq"'::regclass);


--
-- TOC entry 3385 (class 2604 OID 17975)
-- Name: UserRole Id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."UserRole" ALTER COLUMN "Id" SET DEFAULT nextval('public."UserRole_Id_seq"'::regclass);


--
-- TOC entry 3370 (class 2604 OID 17733)
-- Name: help Id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.help ALTER COLUMN "Id" SET DEFAULT nextval('public."help_Id_seq"'::regclass);


--
-- TOC entry 3372 (class 2604 OID 17769)
-- Name: helptxt Id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.helptxt ALTER COLUMN "Id" SET DEFAULT nextval('public."helptxt_Id_seq"'::regclass);


--
-- TOC entry 3373 (class 2604 OID 17785)
-- Name: mailgroup Id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.mailgroup ALTER COLUMN "Id" SET DEFAULT nextval('public."mailgroup_Id_seq"'::regclass);


--
-- TOC entry 3381 (class 2604 OID 17913)
-- Name: mailgrouptxt Id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.mailgrouptxt ALTER COLUMN "Id" SET DEFAULT nextval('public."mailgrouptxt_Id_seq"'::regclass);


--
-- TOC entry 3374 (class 2604 OID 17797)
-- Name: mailtemplate Id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.mailtemplate ALTER COLUMN "Id" SET DEFAULT nextval('public."mailtemplate_Id_seq"'::regclass);


--
-- TOC entry 3837 (class 0 OID 0)
-- Dependencies: 282
-- Name: ChangeLogDetail_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public."ChangeLogDetail_Id_seq"', 82797, true);


--
-- TOC entry 3838 (class 0 OID 0)
-- Dependencies: 220
-- Name: ChangeLog_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public."ChangeLog_Id_seq"', 82798, true);


--
-- TOC entry 3839 (class 0 OID 0)
-- Dependencies: 222
-- Name: ChatMessage_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public."ChatMessage_Id_seq"', 20324, true);


--
-- TOC entry 3840 (class 0 OID 0)
-- Dependencies: 228
-- Name: ChatRoomChatFile_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public."ChatRoomChatFile_Id_seq"', 10087, true);


--
-- TOC entry 3841 (class 0 OID 0)
-- Dependencies: 230
-- Name: ChatRoomChatUserTagged_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public."ChatRoomChatUserTagged_Id_seq"', 10033, true);


--
-- TOC entry 3842 (class 0 OID 0)
-- Dependencies: 226
-- Name: ChatRoomChat_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public."ChatRoomChat_Id_seq"', 10221, true);


--
-- TOC entry 3843 (class 0 OID 0)
-- Dependencies: 224
-- Name: ChatRoom_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public."ChatRoom_Id_seq"', 10027, true);


--
-- TOC entry 3844 (class 0 OID 0)
-- Dependencies: 232
-- Name: Friendship_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public."Friendship_Id_seq"', 20032, true);


--
-- TOC entry 3845 (class 0 OID 0)
-- Dependencies: 238
-- Name: LanguageText_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public."LanguageText_Id_seq"', 37, true);


--
-- TOC entry 3846 (class 0 OID 0)
-- Dependencies: 234
-- Name: Language_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public."Language_Id_seq"', 3136, true);


--
-- TOC entry 3847 (class 0 OID 0)
-- Dependencies: 256
-- Name: MailServiceMailAttach_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public."MailServiceMailAttach_Id_seq"', 18, true);


--
-- TOC entry 3848 (class 0 OID 0)
-- Dependencies: 254
-- Name: MailServiceMailBody_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public."MailServiceMailBody_Id_seq"', 40869, true);


--
-- TOC entry 3849 (class 0 OID 0)
-- Dependencies: 250
-- Name: MailServiceMailConfig_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public."MailServiceMailConfig_Id_seq"', 3, true);


--
-- TOC entry 3850 (class 0 OID 0)
-- Dependencies: 252
-- Name: MailServiceMailStatus_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public."MailServiceMailStatus_Id_seq"', 40869, true);


--
-- TOC entry 3851 (class 0 OID 0)
-- Dependencies: 248
-- Name: MailServiceMail_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public."MailServiceMail_Id_seq"', 40875, true);


--
-- TOC entry 3852 (class 0 OID 0)
-- Dependencies: 246
-- Name: MailServiceRequest_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public."MailServiceRequest_Id_seq"', 40875, true);


--
-- TOC entry 3853 (class 0 OID 0)
-- Dependencies: 262
-- Name: OrgUnitUser_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public."OrgUnitUser_Id_seq"', 10039, true);


--
-- TOC entry 3854 (class 0 OID 0)
-- Dependencies: 260
-- Name: OrgUnit_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public."OrgUnit_Id_seq"', 32, true);


--
-- TOC entry 3855 (class 0 OID 0)
-- Dependencies: 284
-- Name: Permission_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public."Permission_Id_seq"', 52675, true);


--
-- TOC entry 3856 (class 0 OID 0)
-- Dependencies: 264
-- Name: Role_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public."Role_Id_seq"', 20102, true);


--
-- TOC entry 3857 (class 0 OID 0)
-- Dependencies: 268
-- Name: SampleDateData_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public."SampleDateData_Id_seq"', 22, true);


--
-- TOC entry 3858 (class 0 OID 0)
-- Dependencies: 272
-- Name: SettingClient_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public."SettingClient_Id_seq"', 20095, true);


--
-- TOC entry 3859 (class 0 OID 0)
-- Dependencies: 270
-- Name: Setting_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public."Setting_Id_seq"', 30536, true);


--
-- TOC entry 3860 (class 0 OID 0)
-- Dependencies: 276
-- Name: TenantRegistration_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public."TenantRegistration_Id_seq"', 1093, true);


--
-- TOC entry 3861 (class 0 OID 0)
-- Dependencies: 216
-- Name: Tenant_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public."Tenant_Id_seq"', 1054, true);


--
-- TOC entry 3862 (class 0 OID 0)
-- Dependencies: 274
-- Name: UserARN_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public."UserARN_Id_seq"', 1, false);


--
-- TOC entry 3863 (class 0 OID 0)
-- Dependencies: 278
-- Name: UserPasswordHistory_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public."UserPasswordHistory_Id_seq"', 85, true);


--
-- TOC entry 3864 (class 0 OID 0)
-- Dependencies: 280
-- Name: UserReset_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public."UserReset_Id_seq"', 46, true);


--
-- TOC entry 3865 (class 0 OID 0)
-- Dependencies: 266
-- Name: UserRole_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public."UserRole_Id_seq"', 20104, true);


--
-- TOC entry 3866 (class 0 OID 0)
-- Dependencies: 218
-- Name: User_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public."User_Id_seq"', 10098, true);


--
-- TOC entry 3867 (class 0 OID 0)
-- Dependencies: 288
-- Name: auditlog_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.auditlog_id_seq', 1, false);


--
-- TOC entry 3868 (class 0 OID 0)
-- Dependencies: 236
-- Name: help_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public."help_Id_seq"', 10063, true);


--
-- TOC entry 3869 (class 0 OID 0)
-- Dependencies: 240
-- Name: helptxt_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public."helptxt_Id_seq"', 10063, true);


--
-- TOC entry 3870 (class 0 OID 0)
-- Dependencies: 242
-- Name: mailgroup_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public."mailgroup_Id_seq"', 20061, true);


--
-- TOC entry 3871 (class 0 OID 0)
-- Dependencies: 258
-- Name: mailgrouptxt_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public."mailgrouptxt_Id_seq"', 20116, true);


--
-- TOC entry 3872 (class 0 OID 0)
-- Dependencies: 244
-- Name: mailtemplate_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public."mailtemplate_Id_seq"', 20187, true);


--
-- TOC entry 3519 (class 2606 OID 18126)
-- Name: BinaryObjects BinaryObjects_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."BinaryObjects"
    ADD CONSTRAINT "BinaryObjects_pkey" PRIMARY KEY ("Id");


--
-- TOC entry 3512 (class 2606 OID 18099)
-- Name: ChangeLogDetail ChangeLogDetail_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."ChangeLogDetail"
    ADD CONSTRAINT "ChangeLogDetail_pkey" PRIMARY KEY ("Id");


--
-- TOC entry 3410 (class 2606 OID 17567)
-- Name: ChangeLog ChangeLog_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."ChangeLog"
    ADD CONSTRAINT "ChangeLog_pkey" PRIMARY KEY ("Id");


--
-- TOC entry 3414 (class 2606 OID 17588)
-- Name: ChatMessage ChatMessage_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."ChatMessage"
    ADD CONSTRAINT "ChatMessage_pkey" PRIMARY KEY ("Id");


--
-- TOC entry 3429 (class 2606 OID 17665)
-- Name: ChatRoomChatFile ChatRoomChatFile_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."ChatRoomChatFile"
    ADD CONSTRAINT "ChatRoomChatFile_pkey" PRIMARY KEY ("Id");


--
-- TOC entry 3431 (class 2606 OID 17677)
-- Name: ChatRoomChatUserTagged ChatRoomChatUserTagged_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."ChatRoomChatUserTagged"
    ADD CONSTRAINT "ChatRoomChatUserTagged_pkey" PRIMARY KEY ("Id");


--
-- TOC entry 3426 (class 2606 OID 17642)
-- Name: ChatRoomChat ChatRoomChat_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."ChatRoomChat"
    ADD CONSTRAINT "ChatRoomChat_pkey" PRIMARY KEY ("Id");


--
-- TOC entry 3418 (class 2606 OID 17619)
-- Name: ChatRoom ChatRoom_TenantId_ChatRoomId_key; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."ChatRoom"
    ADD CONSTRAINT "ChatRoom_TenantId_ChatRoomId_key" UNIQUE ("TenantId", "ChatRoomId");


--
-- TOC entry 3420 (class 2606 OID 17617)
-- Name: ChatRoom ChatRoom_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."ChatRoom"
    ADD CONSTRAINT "ChatRoom_pkey" PRIMARY KEY ("Id");


--
-- TOC entry 3433 (class 2606 OID 17694)
-- Name: Friendship Friendship_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Friendship"
    ADD CONSTRAINT "Friendship_pkey" PRIMARY KEY ("Id");


--
-- TOC entry 3444 (class 2606 OID 17759)
-- Name: LanguageText LanguageText_LanguageId_Key_key; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."LanguageText"
    ADD CONSTRAINT "LanguageText_LanguageId_Key_key" UNIQUE ("LanguageId", "Key");


--
-- TOC entry 3446 (class 2606 OID 17757)
-- Name: LanguageText LanguageText_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."LanguageText"
    ADD CONSTRAINT "LanguageText_pkey" PRIMARY KEY ("Id");


--
-- TOC entry 3437 (class 2606 OID 17723)
-- Name: Language Language_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Language"
    ADD CONSTRAINT "Language_pkey" PRIMARY KEY ("Id");


--
-- TOC entry 3476 (class 2606 OID 17903)
-- Name: MailServiceMailAttach MailServiceMailAttach_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."MailServiceMailAttach"
    ADD CONSTRAINT "MailServiceMailAttach_pkey" PRIMARY KEY ("Id");


--
-- TOC entry 3472 (class 2606 OID 17889)
-- Name: MailServiceMailBody MailServiceMailBody_MailServiceMail_key; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."MailServiceMailBody"
    ADD CONSTRAINT "MailServiceMailBody_MailServiceMail_key" UNIQUE ("MailServiceMail");


--
-- TOC entry 3474 (class 2606 OID 17887)
-- Name: MailServiceMailBody MailServiceMailBody_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."MailServiceMailBody"
    ADD CONSTRAINT "MailServiceMailBody_pkey" PRIMARY KEY ("Id");


--
-- TOC entry 3464 (class 2606 OID 17857)
-- Name: MailServiceMailConfig MailServiceMailConfig_MailServiceMail_key; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."MailServiceMailConfig"
    ADD CONSTRAINT "MailServiceMailConfig_MailServiceMail_key" UNIQUE ("MailServiceMail");


--
-- TOC entry 3466 (class 2606 OID 17855)
-- Name: MailServiceMailConfig MailServiceMailConfig_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."MailServiceMailConfig"
    ADD CONSTRAINT "MailServiceMailConfig_pkey" PRIMARY KEY ("Id");


--
-- TOC entry 3468 (class 2606 OID 17873)
-- Name: MailServiceMailStatus MailServiceMailStatus_MailServiceMail_key; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."MailServiceMailStatus"
    ADD CONSTRAINT "MailServiceMailStatus_MailServiceMail_key" UNIQUE ("MailServiceMail");


--
-- TOC entry 3470 (class 2606 OID 17871)
-- Name: MailServiceMailStatus MailServiceMailStatus_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."MailServiceMailStatus"
    ADD CONSTRAINT "MailServiceMailStatus_pkey" PRIMARY KEY ("Id");


--
-- TOC entry 3462 (class 2606 OID 17841)
-- Name: MailServiceMail MailServiceMail_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."MailServiceMail"
    ADD CONSTRAINT "MailServiceMail_pkey" PRIMARY KEY ("Id");


--
-- TOC entry 3458 (class 2606 OID 17822)
-- Name: MailServiceRequest MailServiceRequest_TenantId_Id_key; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."MailServiceRequest"
    ADD CONSTRAINT "MailServiceRequest_TenantId_Id_key" UNIQUE ("TenantId", "Id");


--
-- TOC entry 3460 (class 2606 OID 17820)
-- Name: MailServiceRequest MailServiceRequest_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."MailServiceRequest"
    ADD CONSTRAINT "MailServiceRequest_pkey" PRIMARY KEY ("Id");


--
-- TOC entry 3484 (class 2606 OID 17948)
-- Name: OrgUnitUser OrgUnitUser_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."OrgUnitUser"
    ADD CONSTRAINT "OrgUnitUser_pkey" PRIMARY KEY ("Id");


--
-- TOC entry 3482 (class 2606 OID 17929)
-- Name: OrgUnit OrgUnit_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."OrgUnit"
    ADD CONSTRAINT "OrgUnit_pkey" PRIMARY KEY ("Id");


--
-- TOC entry 3521 (class 2606 OID 19580)
-- Name: AuditLog PK_AuditLog; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."AuditLog"
    ADD CONSTRAINT "PK_AuditLog" PRIMARY KEY ("Id");


--
-- TOC entry 3515 (class 2606 OID 18113)
-- Name: Permission Permission_Role_Name_key; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Permission"
    ADD CONSTRAINT "Permission_Role_Name_key" UNIQUE ("Role", "Name");


--
-- TOC entry 3517 (class 2606 OID 18111)
-- Name: Permission Permission_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Permission"
    ADD CONSTRAINT "Permission_pkey" PRIMARY KEY ("Id");


--
-- TOC entry 3486 (class 2606 OID 17965)
-- Name: Role Role_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Role"
    ADD CONSTRAINT "Role_pkey" PRIMARY KEY ("Id");


--
-- TOC entry 3490 (class 2606 OID 17994)
-- Name: SampleDateData SampleDateData_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."SampleDateData"
    ADD CONSTRAINT "SampleDateData_pkey" PRIMARY KEY ("Id");


--
-- TOC entry 3496 (class 2606 OID 18028)
-- Name: SettingClient SettingClient_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."SettingClient"
    ADD CONSTRAINT "SettingClient_pkey" PRIMARY KEY ("Id");


--
-- TOC entry 3493 (class 2606 OID 18008)
-- Name: Setting Setting_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Setting"
    ADD CONSTRAINT "Setting_pkey" PRIMARY KEY ("Id");


--
-- TOC entry 3502 (class 2606 OID 18064)
-- Name: TenantRegistration TenantRegistration_TenancyName_key; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."TenantRegistration"
    ADD CONSTRAINT "TenantRegistration_TenancyName_key" UNIQUE ("TenancyName");


--
-- TOC entry 3504 (class 2606 OID 18062)
-- Name: TenantRegistration TenantRegistration_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."TenantRegistration"
    ADD CONSTRAINT "TenantRegistration_pkey" PRIMARY KEY ("Id");


--
-- TOC entry 3402 (class 2606 OID 17543)
-- Name: Tenant Tenant_TenancyName_key; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Tenant"
    ADD CONSTRAINT "Tenant_TenancyName_key" UNIQUE ("TenancyName");


--
-- TOC entry 3404 (class 2606 OID 17541)
-- Name: Tenant Tenant_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Tenant"
    ADD CONSTRAINT "Tenant_pkey" PRIMARY KEY ("Id");


--
-- TOC entry 3498 (class 2606 OID 18048)
-- Name: UserARN UserARN_UserId_key; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."UserARN"
    ADD CONSTRAINT "UserARN_UserId_key" UNIQUE ("UserId");


--
-- TOC entry 3500 (class 2606 OID 18046)
-- Name: UserARN UserARN_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."UserARN"
    ADD CONSTRAINT "UserARN_pkey" PRIMARY KEY ("Id");


--
-- TOC entry 3506 (class 2606 OID 18071)
-- Name: UserPasswordHistory UserPasswordHistory_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."UserPasswordHistory"
    ADD CONSTRAINT "UserPasswordHistory_pkey" PRIMARY KEY ("Id");


--
-- TOC entry 3508 (class 2606 OID 18085)
-- Name: UserReset UserReset_UserId_key; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."UserReset"
    ADD CONSTRAINT "UserReset_UserId_key" UNIQUE ("UserId");


--
-- TOC entry 3510 (class 2606 OID 18083)
-- Name: UserReset UserReset_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."UserReset"
    ADD CONSTRAINT "UserReset_pkey" PRIMARY KEY ("Id");


--
-- TOC entry 3488 (class 2606 OID 17977)
-- Name: UserRole UserRole_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."UserRole"
    ADD CONSTRAINT "UserRole_pkey" PRIMARY KEY ("Id");


--
-- TOC entry 3406 (class 2606 OID 17555)
-- Name: User User_TenantId_UserLogin_key; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."User"
    ADD CONSTRAINT "User_TenantId_UserLogin_key" UNIQUE ("TenantId", "UserLogin");


--
-- TOC entry 3408 (class 2606 OID 17553)
-- Name: User User_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."User"
    ADD CONSTRAINT "User_pkey" PRIMARY KEY ("Id");


--
-- TOC entry 3397 (class 2606 OID 17529)
-- Name: __EFMigrationsHistory __EFMigrationsHistory_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."__EFMigrationsHistory"
    ADD CONSTRAINT "__EFMigrationsHistory_pkey" PRIMARY KEY ("MigrationId");


--
-- TOC entry 3399 (class 2606 OID 17534)
-- Name: __RefactorLog __RefactorLog_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."__RefactorLog"
    ADD CONSTRAINT "__RefactorLog_pkey" PRIMARY KEY ("OperationKey");


--
-- TOC entry 3440 (class 2606 OID 17737)
-- Name: help help_LanguageId_HelpKey_key; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.help
    ADD CONSTRAINT "help_LanguageId_HelpKey_key" UNIQUE ("LanguageId", "HelpKey");


--
-- TOC entry 3442 (class 2606 OID 17735)
-- Name: help help_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.help
    ADD CONSTRAINT help_pkey PRIMARY KEY ("Id");


--
-- TOC entry 3448 (class 2606 OID 17775)
-- Name: helptxt helptxt_help_key; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.helptxt
    ADD CONSTRAINT helptxt_help_key UNIQUE (help);


--
-- TOC entry 3450 (class 2606 OID 17773)
-- Name: helptxt helptxt_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.helptxt
    ADD CONSTRAINT helptxt_pkey PRIMARY KEY ("Id");


--
-- TOC entry 3452 (class 2606 OID 17787)
-- Name: mailgroup mailgroup_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.mailgroup
    ADD CONSTRAINT mailgroup_pkey PRIMARY KEY ("Id");


--
-- TOC entry 3478 (class 2606 OID 17917)
-- Name: mailgrouptxt mailgrouptxt_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.mailgrouptxt
    ADD CONSTRAINT mailgrouptxt_pkey PRIMARY KEY ("Id");


--
-- TOC entry 3454 (class 2606 OID 17803)
-- Name: mailtemplate mailtemplate_mailgroup_mailkey_key; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.mailtemplate
    ADD CONSTRAINT mailtemplate_mailgroup_mailkey_key UNIQUE (mailgroup, mailkey);


--
-- TOC entry 3456 (class 2606 OID 17801)
-- Name: mailtemplate mailtemplate_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.mailtemplate
    ADD CONSTRAINT mailtemplate_pkey PRIMARY KEY ("Id");


--
-- TOC entry 3411 (class 1259 OID 17578)
-- Name: IX_ChangeLog_Table; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_ChangeLog_Table" ON public."ChangeLog" USING btree ("table");


--
-- TOC entry 3412 (class 1259 OID 17579)
-- Name: IX_ChangeLog_TableKey; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_ChangeLog_TableKey" ON public."ChangeLog" USING btree ("table", key);


--
-- TOC entry 3415 (class 1259 OID 17609)
-- Name: IX_ChatMessage_CreationTime; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_ChatMessage_CreationTime" ON public."ChatMessage" USING btree ("CreationTime");


--
-- TOC entry 3416 (class 1259 OID 17610)
-- Name: IX_ChatMessage_UserId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_ChatMessage_UserId" ON public."ChatMessage" USING btree ("TenantId", "UserId");


--
-- TOC entry 3427 (class 1259 OID 17658)
-- Name: IX_ChatRoomChat_CreationTime; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_ChatRoomChat_CreationTime" ON public."ChatRoomChat" USING btree ("TenantId", "CreationTime");


--
-- TOC entry 3421 (class 1259 OID 17630)
-- Name: IX_ChatRoom_ChatRoomId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_ChatRoom_ChatRoomId" ON public."ChatRoom" USING btree ("TenantId", "ChatRoomId");


--
-- TOC entry 3422 (class 1259 OID 17631)
-- Name: IX_ChatRoom_CreationTime; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_ChatRoom_CreationTime" ON public."ChatRoom" USING btree ("TenantId", "CreationTime");


--
-- TOC entry 3423 (class 1259 OID 17632)
-- Name: IX_ChatRoom_Description; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_ChatRoom_Description" ON public."ChatRoom" USING btree ("TenantId", "Description");


--
-- TOC entry 3424 (class 1259 OID 17633)
-- Name: IX_ChatRoom_Name; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_ChatRoom_Name" ON public."ChatRoom" USING btree ("TenantId", "Name");


--
-- TOC entry 3434 (class 1259 OID 17715)
-- Name: IX_Friendship_CreationTime; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_Friendship_CreationTime" ON public."Friendship" USING btree ("CreationTime");


--
-- TOC entry 3435 (class 1259 OID 17716)
-- Name: IX_Friendship_UserId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_Friendship_UserId" ON public."Friendship" USING btree ("TenantId", "UserId");


--
-- TOC entry 3479 (class 1259 OID 17940)
-- Name: IX_OrgUnit_IsDeleted; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_OrgUnit_IsDeleted" ON public."OrgUnit" USING btree ("IsDeleted");


--
-- TOC entry 3480 (class 1259 OID 17941)
-- Name: IX_OrgUnit_Name; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_OrgUnit_Name" ON public."OrgUnit" USING btree ("TenantId", "Name");


--
-- TOC entry 3513 (class 1259 OID 18119)
-- Name: IX_Permission_Name; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_Permission_Name" ON public."Permission" USING btree ("Name");


--
-- TOC entry 3494 (class 1259 OID 18039)
-- Name: IX_SettingClient_ClientType; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_SettingClient_ClientType" ON public."SettingClient" USING btree ("ClientType", "Name");


--
-- TOC entry 3491 (class 1259 OID 18019)
-- Name: IX_Setting_Name; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_Setting_Name" ON public."Setting" USING btree ("Name");


--
-- TOC entry 3400 (class 1259 OID 17544)
-- Name: IX_Tenant_Name; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_Tenant_Name" ON public."Tenant" USING btree ("Name");


--
-- TOC entry 3438 (class 1259 OID 17748)
-- Name: IX_help_code; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_help_code" ON public.help USING btree ("TenantId", "HelpKey");


--
-- TOC entry 3575 (class 2606 OID 19581)
-- Name: AuditLog FK_AuditLog_ImpersonalizerUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."AuditLog"
    ADD CONSTRAINT "FK_AuditLog_ImpersonalizerUserId" FOREIGN KEY ("ImpersonalizerUserId") REFERENCES public."User"("Id");


--
-- TOC entry 3576 (class 2606 OID 19586)
-- Name: AuditLog FK_AuditLog_Tenant; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."AuditLog"
    ADD CONSTRAINT "FK_AuditLog_Tenant" FOREIGN KEY ("TenantId") REFERENCES public."Tenant"("Id");


--
-- TOC entry 3577 (class 2606 OID 19591)
-- Name: AuditLog FK_AuditLog_User; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."AuditLog"
    ADD CONSTRAINT "FK_AuditLog_User" FOREIGN KEY ("UserId") REFERENCES public."User"("Id");


--
-- TOC entry 3574 (class 2606 OID 18127)
-- Name: BinaryObjects FK_BinaryObjects_Tenant; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."BinaryObjects"
    ADD CONSTRAINT "FK_BinaryObjects_Tenant" FOREIGN KEY ("TenantId") REFERENCES public."Tenant"("Id");


--
-- TOC entry 3572 (class 2606 OID 18100)
-- Name: ChangeLogDetail "FK_ChangeLogDetail_ChangeLog"; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."ChangeLogDetail"
    ADD CONSTRAINT "FK_ChangeLogDetail_ChangeLog" FOREIGN KEY (changelog) REFERENCES public."ChangeLog"("Id");


--
-- TOC entry 3523 (class 2606 OID 17573)
-- Name: ChangeLog FK_ChangeLog_Tenant; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."ChangeLog"
    ADD CONSTRAINT "FK_ChangeLog_Tenant" FOREIGN KEY ("TenantId") REFERENCES public."Tenant"("Id");


--
-- TOC entry 3524 (class 2606 OID 17568)
-- Name: ChangeLog FK_ChangeLog_User; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."ChangeLog"
    ADD CONSTRAINT "FK_ChangeLog_User" FOREIGN KEY ("UserId") REFERENCES public."User"("Id");


--
-- TOC entry 3525 (class 2606 OID 17599)
-- Name: ChatMessage FK_ChatMessage_FriendTenantId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."ChatMessage"
    ADD CONSTRAINT "FK_ChatMessage_FriendTenantId" FOREIGN KEY ("FriendTenantId") REFERENCES public."Tenant"("Id");


--
-- TOC entry 3526 (class 2606 OID 17604)
-- Name: ChatMessage FK_ChatMessage_FriendUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."ChatMessage"
    ADD CONSTRAINT "FK_ChatMessage_FriendUserId" FOREIGN KEY ("FriendUserId") REFERENCES public."User"("Id");


--
-- TOC entry 3527 (class 2606 OID 17589)
-- Name: ChatMessage FK_ChatMessage_TenantId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."ChatMessage"
    ADD CONSTRAINT "FK_ChatMessage_TenantId" FOREIGN KEY ("TenantId") REFERENCES public."Tenant"("Id");


--
-- TOC entry 3528 (class 2606 OID 17594)
-- Name: ChatMessage FK_ChatMessage_UserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."ChatMessage"
    ADD CONSTRAINT "FK_ChatMessage_UserId" FOREIGN KEY ("UserId") REFERENCES public."User"("Id");


--
-- TOC entry 3531 (class 2606 OID 17648)
-- Name: ChatRoomChat FK_ChatRoomChat_ChatRoom; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."ChatRoomChat"
    ADD CONSTRAINT "FK_ChatRoomChat_ChatRoom" FOREIGN KEY ("ChatRoom") REFERENCES public."ChatRoom"("Id");


--
-- TOC entry 3534 (class 2606 OID 17666)
-- Name: ChatRoomChatFile FK_ChatRoomChatFile_ChatRoomChat; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."ChatRoomChatFile"
    ADD CONSTRAINT "FK_ChatRoomChatFile_ChatRoomChat" FOREIGN KEY ("ChatRoomChat") REFERENCES public."ChatRoomChat"("Id");


--
-- TOC entry 3532 (class 2606 OID 17653)
-- Name: ChatRoomChat FK_ChatRoomChat_Tenant; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."ChatRoomChat"
    ADD CONSTRAINT "FK_ChatRoomChat_Tenant" FOREIGN KEY ("TenantId") REFERENCES public."Tenant"("Id");


--
-- TOC entry 3533 (class 2606 OID 17643)
-- Name: ChatRoomChat FK_ChatRoomChat_User; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."ChatRoomChat"
    ADD CONSTRAINT "FK_ChatRoomChat_User" FOREIGN KEY ("User") REFERENCES public."User"("Id");


--
-- TOC entry 3535 (class 2606 OID 17683)
-- Name: ChatRoomChatUserTagged FK_ChatRoomChatUserTagged_ChatRoomChat; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."ChatRoomChatUserTagged"
    ADD CONSTRAINT "FK_ChatRoomChatUserTagged_ChatRoomChat" FOREIGN KEY ("ChatRoomChat") REFERENCES public."ChatRoomChat"("Id");


--
-- TOC entry 3536 (class 2606 OID 17678)
-- Name: ChatRoomChatUserTagged FK_ChatRoomChatUserTagged_User; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."ChatRoomChatUserTagged"
    ADD CONSTRAINT "FK_ChatRoomChatUserTagged_User" FOREIGN KEY ("UserTagged") REFERENCES public."User"("Id");


--
-- TOC entry 3529 (class 2606 OID 17620)
-- Name: ChatRoom FK_ChatRoom_Tenant; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."ChatRoom"
    ADD CONSTRAINT "FK_ChatRoom_Tenant" FOREIGN KEY ("TenantId") REFERENCES public."Tenant"("Id");


--
-- TOC entry 3530 (class 2606 OID 17625)
-- Name: ChatRoom FK_ChatRoom_User; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."ChatRoom"
    ADD CONSTRAINT "FK_ChatRoom_User" FOREIGN KEY ("UserCreator") REFERENCES public."User"("Id");


--
-- TOC entry 3537 (class 2606 OID 17705)
-- Name: Friendship FK_Friendship_FriendTenantId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Friendship"
    ADD CONSTRAINT "FK_Friendship_FriendTenantId" FOREIGN KEY ("FriendTenantId") REFERENCES public."Tenant"("Id");


--
-- TOC entry 3538 (class 2606 OID 17710)
-- Name: Friendship FK_Friendship_FriendUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Friendship"
    ADD CONSTRAINT "FK_Friendship_FriendUserId" FOREIGN KEY ("FriendUserId") REFERENCES public."User"("Id");


--
-- TOC entry 3539 (class 2606 OID 17695)
-- Name: Friendship FK_Friendship_TenantId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Friendship"
    ADD CONSTRAINT "FK_Friendship_TenantId" FOREIGN KEY ("TenantId") REFERENCES public."Tenant"("Id");


--
-- TOC entry 3540 (class 2606 OID 17700)
-- Name: Friendship FK_Friendship_UserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Friendship"
    ADD CONSTRAINT "FK_Friendship_UserId" FOREIGN KEY ("UserId") REFERENCES public."User"("Id");


--
-- TOC entry 3542 (class 2606 OID 17738)
-- Name: help FK_help_Language; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.help
    ADD CONSTRAINT "FK_help_Language" FOREIGN KEY ("LanguageId") REFERENCES public."Language"("Id");


--
-- TOC entry 3543 (class 2606 OID 17743)
-- Name: help FK_help_Tenant; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.help
    ADD CONSTRAINT "FK_help_Tenant" FOREIGN KEY ("TenantId") REFERENCES public."Tenant"("Id");


--
-- TOC entry 3545 (class 2606 OID 17776)
-- Name: helptxt FK_helptxt_help; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.helptxt
    ADD CONSTRAINT "FK_helptxt_help" FOREIGN KEY (help) REFERENCES public.help("Id");


--
-- TOC entry 3541 (class 2606 OID 17724)
-- Name: Language FK_Language_Tenant; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Language"
    ADD CONSTRAINT "FK_Language_Tenant" FOREIGN KEY ("TenantId") REFERENCES public."Tenant"("Id");


--
-- TOC entry 3544 (class 2606 OID 17760)
-- Name: LanguageText FK_LanguageText_Language; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."LanguageText"
    ADD CONSTRAINT "FK_LanguageText_Language" FOREIGN KEY ("LanguageId") REFERENCES public."Language"("Id");


--
-- TOC entry 3546 (class 2606 OID 17788)
-- Name: mailgroup FK_mailgroup_Tenant; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.mailgroup
    ADD CONSTRAINT "FK_mailgroup_Tenant" FOREIGN KEY ("TenantId") REFERENCES public."Tenant"("Id");


--
-- TOC entry 3556 (class 2606 OID 17918)
-- Name: mailgrouptxt FK_mailgrouptxt_mailgroup; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.mailgrouptxt
    ADD CONSTRAINT "FK_mailgrouptxt_mailgroup" FOREIGN KEY (mailgroup) REFERENCES public.mailgroup("Id");


--
-- TOC entry 3555 (class 2606 OID 17904)
-- Name: MailServiceMailAttach FK_MailServiceMailAttach_MailServiceMailBody; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."MailServiceMailAttach"
    ADD CONSTRAINT "FK_MailServiceMailAttach_MailServiceMailBody" FOREIGN KEY ("MailServiceMailBody") REFERENCES public."MailServiceMailBody"("Id");


--
-- TOC entry 3554 (class 2606 OID 17890)
-- Name: MailServiceMailBody FK_MailServiceMailBody_MailServiceMail; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."MailServiceMailBody"
    ADD CONSTRAINT "FK_MailServiceMailBody_MailServiceMail" FOREIGN KEY ("MailServiceMail") REFERENCES public."MailServiceMail"("Id");


--
-- TOC entry 3552 (class 2606 OID 17858)
-- Name: MailServiceMailConfig FK_MailServiceMailConfig_MailServiceMail; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."MailServiceMailConfig"
    ADD CONSTRAINT "FK_MailServiceMailConfig_MailServiceMail" FOREIGN KEY ("MailServiceMail") REFERENCES public."MailServiceMail"("Id");


--
-- TOC entry 3551 (class 2606 OID 17842)
-- Name: MailServiceMail FK_MailServiceMail_MailServiceRequest; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."MailServiceMail"
    ADD CONSTRAINT "FK_MailServiceMail_MailServiceRequest" FOREIGN KEY ("TenantId", "MailServiceRequest") REFERENCES public."MailServiceRequest"("TenantId", "Id");


--
-- TOC entry 3553 (class 2606 OID 17874)
-- Name: MailServiceMailStatus FK_MailServiceMailStatus_MailServiceMail; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."MailServiceMailStatus"
    ADD CONSTRAINT "FK_MailServiceMailStatus_MailServiceMail" FOREIGN KEY ("MailServiceMail") REFERENCES public."MailServiceMail"("Id");


--
-- TOC entry 3549 (class 2606 OID 17823)
-- Name: MailServiceRequest FK_MailServiceRequest_Tenant; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."MailServiceRequest"
    ADD CONSTRAINT "FK_MailServiceRequest_Tenant" FOREIGN KEY ("TenantId") REFERENCES public."Tenant"("Id");


--
-- TOC entry 3550 (class 2606 OID 17828)
-- Name: MailServiceRequest FK_MailServiceRequest_User; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."MailServiceRequest"
    ADD CONSTRAINT "FK_MailServiceRequest_User" FOREIGN KEY ("UserCreator") REFERENCES public."User"("Id");


--
-- TOC entry 3547 (class 2606 OID 17804)
-- Name: mailtemplate FK_mailtemplate_mailgroup; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.mailtemplate
    ADD CONSTRAINT "FK_mailtemplate_mailgroup" FOREIGN KEY (mailgroup) REFERENCES public.mailgroup("Id");


--
-- TOC entry 3548 (class 2606 OID 17809)
-- Name: mailtemplate FK_mailtemplate_Tenant; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.mailtemplate
    ADD CONSTRAINT "FK_mailtemplate_Tenant" FOREIGN KEY ("TenantId") REFERENCES public."Tenant"("Id");


--
-- TOC entry 3557 (class 2606 OID 17930)
-- Name: OrgUnit FK_OrgUnit_OrgUnit; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."OrgUnit"
    ADD CONSTRAINT "FK_OrgUnit_OrgUnit" FOREIGN KEY ("ParentOU") REFERENCES public."OrgUnit"("Id");


--
-- TOC entry 3558 (class 2606 OID 17935)
-- Name: OrgUnit FK_OrgUnit_Tenant; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."OrgUnit"
    ADD CONSTRAINT "FK_OrgUnit_Tenant" FOREIGN KEY ("TenantId") REFERENCES public."Tenant"("Id");


--
-- TOC entry 3559 (class 2606 OID 17949)
-- Name: OrgUnitUser FK_OrgUnitUser_OrgUnit; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."OrgUnitUser"
    ADD CONSTRAINT "FK_OrgUnitUser_OrgUnit" FOREIGN KEY ("OrgUnit") REFERENCES public."OrgUnit"("Id");


--
-- TOC entry 3560 (class 2606 OID 17954)
-- Name: OrgUnitUser FK_OrgUnitUser_User; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."OrgUnitUser"
    ADD CONSTRAINT "FK_OrgUnitUser_User" FOREIGN KEY ("User") REFERENCES public."User"("Id");


--
-- TOC entry 3573 (class 2606 OID 18114)
-- Name: Permission FK_Permission_Role; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Permission"
    ADD CONSTRAINT "FK_Permission_Role" FOREIGN KEY ("Role") REFERENCES public."Role"("Id");


--
-- TOC entry 3561 (class 2606 OID 17966)
-- Name: Role FK_Role_Tenant; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Role"
    ADD CONSTRAINT "FK_Role_Tenant" FOREIGN KEY ("TenantId") REFERENCES public."Tenant"("Id");


--
-- TOC entry 3564 (class 2606 OID 17995)
-- Name: SampleDateData FK_SampleDateData_Tenant; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."SampleDateData"
    ADD CONSTRAINT "FK_SampleDateData_Tenant" FOREIGN KEY ("TenantId") REFERENCES public."Tenant"("Id");


--
-- TOC entry 3567 (class 2606 OID 18029)
-- Name: SettingClient FK_SettingClient_Tenant; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."SettingClient"
    ADD CONSTRAINT "FK_SettingClient_Tenant" FOREIGN KEY ("TenantId") REFERENCES public."Tenant"("Id");


--
-- TOC entry 3568 (class 2606 OID 18034)
-- Name: SettingClient FK_SettingClient_User; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."SettingClient"
    ADD CONSTRAINT "FK_SettingClient_User" FOREIGN KEY ("UserId") REFERENCES public."User"("Id");


--
-- TOC entry 3565 (class 2606 OID 18009)
-- Name: Setting FK_Setting_Tenant; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Setting"
    ADD CONSTRAINT "FK_Setting_Tenant" FOREIGN KEY ("TenantId") REFERENCES public."Tenant"("Id");


--
-- TOC entry 3566 (class 2606 OID 18014)
-- Name: Setting FK_Setting_User; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Setting"
    ADD CONSTRAINT "FK_Setting_User" FOREIGN KEY ("UserId") REFERENCES public."User"("Id");


--
-- TOC entry 3569 (class 2606 OID 18049)
-- Name: UserARN FK_UserARN_User; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."UserARN"
    ADD CONSTRAINT "FK_UserARN_User" FOREIGN KEY ("UserId") REFERENCES public."User"("Id");


--
-- TOC entry 3570 (class 2606 OID 18072)
-- Name: UserPasswordHistory FK_UserPasswordHistory_User; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."UserPasswordHistory"
    ADD CONSTRAINT "FK_UserPasswordHistory_User" FOREIGN KEY ("UserId") REFERENCES public."User"("Id");


--
-- TOC entry 3571 (class 2606 OID 18086)
-- Name: UserReset FK_UserReset_User; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."UserReset"
    ADD CONSTRAINT "FK_UserReset_User" FOREIGN KEY ("UserId") REFERENCES public."User"("Id");


--
-- TOC entry 3562 (class 2606 OID 17983)
-- Name: UserRole FK_UserRole_Role; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."UserRole"
    ADD CONSTRAINT "FK_UserRole_Role" FOREIGN KEY ("RoleId") REFERENCES public."Role"("Id");


--
-- TOC entry 3563 (class 2606 OID 17978)
-- Name: UserRole FK_UserRole_User; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."UserRole"
    ADD CONSTRAINT "FK_UserRole_User" FOREIGN KEY ("UserId") REFERENCES public."User"("Id");


--
-- TOC entry 3522 (class 2606 OID 17556)
-- Name: User FK_User_Tenant; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."User"
    ADD CONSTRAINT "FK_User_Tenant" FOREIGN KEY ("TenantId") REFERENCES public."Tenant"("Id");






--------------------------------------------------------------
-- Carga de datos iniciales
--------------------------------------------------------------


DO $$

DECLARE admonName VARCHAR(13) := 'Administrador';
DECLARE strDefault VARCHAR(7) := 'Default';
DECLARE strFalse VARCHAR(5) := 'false';

BEGIN

		INSERT INTO public."User" ("Id", "TenantId", "UserLogin", "Password", "Name", "Lastname", "SecondLastname", "EmailAddress", "IsEmailConfirmed", "PhoneNumber"
			,"IsPhoneConfirmed", "CreationTime", "ChangePassword", "AccessFailedCount", "ProfilePictureId", "UserLocked", "IsLockoutEnabled", "IsActive", "IsDeleted")
		VALUES
			(1, NULL, 'admin', 'AQAAAAEAACcQAAAAECqqu++Q+dyd14CAgQ09k26e/abCYTIm/VjrVN0ScQSPs7896bkb4dBTgbgE7ZW82A==', admonName, 'Host', NULL, 'user@correo.com.mx'
			, true, NULL, false, now(), true, 0, NULL, false, true, true, false);

		
		INSERT INTO public."Tenant" ("Id", "TenancyName", "Name", "CreationTime", "IsActive", "IsDeleted")
		VALUES (1, strDefault, strDefault, now(), true, false);



		INSERT INTO public."User" ("Id", "TenantId", "UserLogin", "Password", "Name", "Lastname", "SecondLastname", "EmailAddress", "IsEmailConfirmed", "PhoneNumber"
			,"IsPhoneConfirmed", "CreationTime", "ChangePassword", "AccessFailedCount", "ProfilePictureId", "UserLocked", "IsLockoutEnabled", "IsActive", "IsDeleted")
		VALUES
			(2, 1, 'admin', 'AQAAAAEAACcQAAAAECqqu++Q+dyd14CAgQ09k26e/abCYTIm/VjrVN0ScQSPs7896bkb4dBTgbgE7ZW82A==', admonName, 'Tenant', strDefault, 'user@algoria.com.mx'
			, true, NULL, false, now(), true, 0, NULL, false, true, true, false);


		PERFORM setval(pg_get_serial_sequence('public."User"', 'Id'), (SELECT MAX("Id") FROM public."User"));
		PERFORM setval(pg_get_serial_sequence('public."Tenant"', 'Id'), (SELECT MAX("Id") FROM public."Tenant"));


		INSERT INTO public."Language" ("Id", "TenantId", "Name", "DisplayName", "IsActive")
		VALUES (1, NULL, 'es-MX', 'Español (México)', true);

		INSERT INTO public."Language" ("Id", "TenantId", "Name", "DisplayName", "IsActive")
		VALUES (2, 1, 'es-MX', 'Español (México)', true);

		PERFORM setval(pg_get_serial_sequence('public."Language"', 'Id'), (SELECT MAX("Id") FROM public."Language"));


		INSERT INTO public."Role" ("Id", "TenantId", "Name", "DisplayName", "IsActive", "IsDeleted")
		VALUES (1, NULL, 'Admin', admonName, true, false);

		INSERT INTO public."Role" ("Id", "TenantId", "Name", "DisplayName", "IsActive", "IsDeleted")
		VALUES (2, 1, 'Admin', admonName, true, false);

		PERFORM setval(pg_get_serial_sequence('public."Role"', 'Id'), (SELECT MAX("Id") FROM public."Role"));

		INSERT INTO public."Permission" ("Role", "Name", "IsGranted") VALUES 
		(1, 'Pages', true),
		(1, 'Pages.Administration', true),
		(1, 'Pages.Administration.Roles', true),
		(1, 'Pages.Administration.Roles.Create', true),
		(1, 'Pages.Administration.Roles.Edit',true),
		(1, 'Pages.Administration.Roles.Delete', true),
		(1, 'Pages.Administration.Users', true),
		(1, 'Pages.Administration.Users.Create', true),
		(1, 'Pages.Administration.Users.Edit', true),
		(1, 'Pages.Administration.Users.Delete', true),
		(1, 'Pages.Administration.Users.ChangePermissions', true),
		(1, 'Pages.Administration.Users.Impersonation', true),
		(1, 'Pages.Administration.Languages', true),
		(1, 'Pages.Administration.Languages.Create', true),
		(1, 'Pages.Administration.Languages.Edit', true),
		(1, 'Pages.Administration.Languages.Delete', true),
		(1, 'Pages.Administration.Languages.ChangeTexts', true),
		(1, 'Pages.Administration.AuditLogs', true),
		(1, 'Pages.Administration.Host.Settings', true),
		(1, 'Pages.Administration.Host.Maintenance', true),
		(1, 'Pages.Tenants', true),
		(1, 'Pages.Tenants.Create', true),
		(1, 'Pages.Tenants.Edit', true),
		(1, 'Pages.Tenants.Delete', true),
		(1, 'Pages.Tenants.Impersonation', true),
		(1, 'hosthost.1', true),
		(1, 'conftcor.0', true),
		(1, 'conftcor.1', true),
		(1, 'conftcor.2', true),
		(1, 'conftcor.3', true),
		(1, 'conftcor.4', true),
		(1, 'conftcor.5', true),
		(1, 'conftcor.6', true),
		(2, 'Pages', true),
		(2, 'Pages.Administration', true),
		(2, 'Pages.Administration.Roles', true),
		(2, 'Pages.Administration.Roles.Create', true),
		(2, 'Pages.Administration.Roles.Edit', true),
		(2, 'Pages.Administration.Roles.Delete', true),
		(2, 'Pages.Administration.Users', true),
		(2, 'Pages.Administration.Users.Create', true),
		(2, 'Pages.Administration.Users.Edit', true),
		(2, 'Pages.Administration.Users.Delete', true),
		(2, 'Pages.Administration.Users.ChangePermissions', true),
		(2, 'Pages.Administration.Users.Impersonation', true),
		(2, 'Pages.Administration.Languages', true),
		(2, 'Pages.Administration.Languages.Create', true),
		(2, 'Pages.Administration.Languages.Edit', true),
		(2, 'Pages.Administration.Languages.Delete', true),
		(2, 'Pages.Administration.Languages.ChangeTexts', true),
		(2, 'Pages.Administration.AuditLogs', true),
		(2, 'Pages.Administration.Tenant.Settings', true),
		(2, 'Pages.Tenant.Dashboard', true),
		(2, 'Pages.Tenant.Catalogos', true),
		(2, 'conftcor.0', true),
		(2, 'conftcor.1', true),
		(2, 'conftcor.2', true),
		(2, 'conftcor.3', true),
		(2, 'conftcor.4', true),
		(2, 'conftcor.5', true),
		(2, 'conftcor.6', true);



		INSERT INTO public."UserRole" ("Id", "UserId", "RoleId")
		VALUES (1, 1, 1);

		INSERT INTO public."UserRole" ("Id", "UserId", "RoleId")
		VALUES (2, 2, 2);


		PERFORM setval(pg_get_serial_sequence('public."UserRole"', 'Id'), (SELECT MAX("Id") FROM public."UserRole"));
		

		INSERT INTO public."Setting" ("TenantId", "Name", "value")
		VALUES (NULL, 'LanguageDefault', '1');

		INSERT INTO public."Setting" ("TenantId", "Name", "value")
		VALUES (NULL, 'WebSiteRootAddress', 'http://localhost:8089/');

		INSERT INTO public."Setting" ("TenantId", "Name", "value")
		VALUES (NULL, 'EnableUserBlocking', strFalse);

		INSERT INTO public."Setting" ("TenantId", "Name", "value")
		VALUES (NULL, 'FailedAttemptsToBlockUser', '5');

		INSERT INTO public."Setting" ("TenantId", "Name", "value")
		VALUES (NULL, 'UserBlockingDuration', '300');

		INSERT INTO public."Setting" ("TenantId", "Name", "value")
		VALUES (NULL, 'EnableTwoFactorLogin', strFalse);

		INSERT INTO public."Setting" ("TenantId", "Name", "value")
		VALUES (NULL, 'EnableMailVerification', strFalse);

		INSERT INTO public."Setting" ("TenantId", "Name", "value")
		VALUES (NULL, 'EnableSMSVerification', strFalse);

		INSERT INTO public."Setting" ("TenantId", "Name", "value")
		VALUES (NULL, 'EnableBrowserRemenberMe', strFalse);

		INSERT INTO public."Setting" ("TenantId", "Name", "value")
		VALUES (NULL, 'MailEnableSSL', strFalse);

		INSERT INTO public."Setting" ("TenantId", "Name", "value")
		VALUES (NULL, 'MailUseDefaultCredentials', 'true');

		INSERT INTO public."Setting" ("TenantId", "Name", "value")
		VALUES (NULL, 'MailGroup', '1');

		INSERT INTO public."Setting" ("TenantId", "Name", "value")
		VALUES (1, 'LanguageDefault', '2');

		INSERT INTO public."Setting" ("TenantId", "Name", "value")
		VALUES (1, 'EnableUserBlocking', strFalse);

		INSERT INTO public."Setting" ("TenantId", "Name", "value")
		VALUES (1, 'FailedAttemptsToBlockUser', '5');

		INSERT INTO public."Setting" ("TenantId", "Name", "value")
		VALUES (1, 'UserBlockingDuration', '300');

		INSERT INTO public."Setting" ("TenantId", "Name", "value")
		VALUES (1, 'EnablePasswordPeriod', strFalse);

		INSERT INTO public."Setting" ("TenantId", "Name", "value")
		VALUES (1, 'PasswordValidDays', '30');

		INSERT INTO public."Setting" ("TenantId", "Name", "value")
		VALUES (1, 'MailEnableSSL', strFalse);

		INSERT INTO public."Setting" ("TenantId", "Name", "value")
		VALUES (1, 'MailUseDefaultCredentials', 'true');

		INSERT INTO public."Setting" ("TenantId", "Name", "value")
		VALUES (1, 'MailGroup', '2');



		INSERT INTO public."mailgroup" ("Id", "TenantId", "DisplayName")
		VALUES (1, NULL, 'Tema de correos default');

		INSERT INTO public."mailgroup" ("Id", "TenantId", "DisplayName")
		VALUES (2, 1, 'Tema de correos default');

		PERFORM setval(pg_get_serial_sequence('public."mailgroup"', 'Id'), (SELECT MAX("Id") FROM public."mailgroup"));


		INSERT INTO public."mailtemplate" ("TenantId", "mailgroup", "mailkey", "DisplayName", "Subject", "IsActive")
		VALUES (NULL, 1, 'usr-nuevo', 'Plantilla Nuevo usuario', 'Nuevo Usuario', true);

		INSERT INTO public."mailtemplate" ("TenantId", "mailgroup", "mailkey", "DisplayName", "Subject", "IsActive")
		VALUES (NULL, 1, 'usr-reset', 'Plantilla Reset Contraseña por Usuario', 'Reset de Contraseña por usuario', true);

		INSERT INTO public."mailtemplate" ("TenantId", "mailgroup", "mailkey", "DisplayName", "Subject", "IsActive")
		VALUES (NULL, 1, 'usr-unblock', 'Plantilla Desbloquear Usuario', 'Desbloquear Usuario', true);

		INSERT INTO public."mailtemplate" ("TenantId", "mailgroup", "mailkey", "DisplayName", "Subject", "IsActive")
		VALUES (NULL, 1, 'usr-modi', 'Plantilla Modificar Contraseña por Administrador', 'Modificar Contraseña por Administrador', true);

		INSERT INTO public."mailtemplate" ("TenantId", "mailgroup", "mailkey", "DisplayName", "Subject", "IsActive")
		VALUES (NULL, 1, 'tenant-reg', 'Plantilla Registrar tenant', 'Registrar tenant', true);

		INSERT INTO public."mailtemplate" ("TenantId", "mailgroup", "mailkey", "DisplayName", "Subject", "IsActive")
		VALUES (1, 2, 'usr-nuevo', 'Plantilla Nuevo usuario', 'Nuevo Usuario', true);

		INSERT INTO public."mailtemplate" ("TenantId", "mailgroup", "mailkey", "DisplayName", "Subject", "IsActive")
		VALUES (1, 2, 'usr-reset', 'Plantilla Reset Contraseña por Usuario', 'Reset de Contraseña por usuario', true);

		INSERT INTO public."mailtemplate" ("TenantId", "mailgroup", "mailkey", "DisplayName", "Subject", "IsActive")
		VALUES (1, 2, 'usr-unblock', 'Plantilla Desbloquear Usuario', 'Desbloquear Usuario', true);

		INSERT INTO public."mailtemplate" ("TenantId", "mailgroup", "mailkey", "DisplayName", "Subject", "IsActive")
		VALUES (1, 2, 'usr-modi', 'Plantilla Modificar Contraseña por Administrador', 'Modificar Contraseña por Administrador', true);



END $$;


