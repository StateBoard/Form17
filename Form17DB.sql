USE [Form17_2022]
GO
/****** Object:  Table [dbo].[Final]    Script Date: 01/11/2021 10:58:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Final](
	[Index_No] [nvarchar](255) NULL,
	[NAME] [nvarchar](255) NULL,
	[DIV_Code] [float] NULL,
	[Division_Name] [nvarchar](255) NULL,
	[STREAM] [nvarchar](255) NULL,
	[MEDIUM] [nvarchar](255) NULL,
	[STD] [float] NULL,
	[Count] [float] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[HSC_Final]    Script Date: 01/11/2021 10:58:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HSC_Final](
	[ID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Sheet1]    Script Date: 01/11/2021 10:58:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sheet1](
	[Sr_No] [float] NULL,
	[Index_No] [float] NULL,
	[Medium] [nvarchar](255) NULL,
	[Dist] [nvarchar](255) NULL,
	[Taluka] [ntext] NULL,
	[Name] [ntext] NULL,
	[Pin_Code] [float] NULL,
	[Mobile_No] [float] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Sheet2]    Script Date: 01/11/2021 10:58:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sheet2](
	[S_ No] [nvarchar](50) NULL,
	[Index_No] [nvarchar](50) NULL,
	[Medium] [nvarchar](255) NULL,
	[Dist] [nvarchar](255) NULL,
	[Taluka] [nvarchar](max) NULL,
	[Name] [nvarchar](max) NULL,
	[Pin_Code] [nvarchar](max) NULL,
	[Mobile_No] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tbl_Center]    Script Date: 01/11/2021 10:58:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tbl_Center](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Index_No] [nvarchar](10) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[Address] [nvarchar](max) NULL,
	[Pin] [nvarchar](10) NULL,
	[Div_Code] [nvarchar](5) NULL,
	[Division_Name] [nvarchar](max) NULL,
	[STD] [int] NULL,
	[Count] [int] NULL,
 CONSTRAINT [PK_Tbl_Center] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tbl_Center_Details]    Script Date: 01/11/2021 10:58:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tbl_Center_Details](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Index_No] [nvarchar](10) NOT NULL,
	[STD] [int] NULL,
	[Stream] [nvarchar](200) NULL,
	[Medium] [nvarchar](200) NULL,
 CONSTRAINT [PK_Tbl_Center_Details] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tbl_Code_Master]    Script Date: 01/11/2021 10:58:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tbl_Code_Master](
	[id] [int] NOT NULL,
	[division_code] [nvarchar](max) NULL,
	[division_name] [nvarchar](max) NULL,
	[district_code] [nvarchar](max) NULL,
	[district_name] [nvarchar](max) NULL,
	[taluka_code] [nvarchar](max) NULL,
	[taluka_name] [nvarchar](max) NULL,
 CONSTRAINT [PK_Tbl_Code_Master] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tbl_Colmst]    Script Date: 01/11/2021 10:58:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tbl_Colmst](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[SCHNO] [nvarchar](255) NULL,
	[NAME] [nvarchar](255) NULL,
	[DIV,C,1] [nvarchar](255) NULL,
	[STREAM] [nvarchar](255) NULL,
	[MEDIUM] [nvarchar](255) NULL,
	[Count] [float] NULL,
	[Active] [float] NULL,
 CONSTRAINT [PK_Tbl_Colmst] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tbl_HSC_Form17]    Script Date: 01/11/2021 10:58:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tbl_HSC_Form17](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Last_Name] [nvarchar](max) NULL,
	[First_Name] [nvarchar](max) NULL,
	[Father_Husband_Name] [nvarchar](max) NULL,
	[Mother_Name] [nvarchar](max) NULL,
	[Gender] [nvarchar](10) NULL,
	[Residential_Address] [nvarchar](max) NULL,
	[Pincode] [nvarchar](6) NULL,
	[District] [nvarchar](2) NULL,
	[Taluka] [nvarchar](2) NULL,
	[State] [nvarchar](50) NULL,
	[Aadhar_No] [nvarchar](12) NULL,
	[Mobile_No] [nvarchar](10) NULL,
	[Email_ID] [nvarchar](max) NULL,
	[DOB] [nvarchar](max) NULL,
	[Village_of_Birth] [nvarchar](max) NULL,
	[Taluka_of_Birth] [nvarchar](max) NULL,
	[District_of_Birth] [nvarchar](max) NULL,
	[SSC_Passing_Month] [nvarchar](50) NULL,
	[SSC_Passing_Year] [nvarchar](5) NULL,
	[SSC_Division_Board] [nvarchar](50) NULL,
	[Other_SSC_Board] [nvarchar](max) NULL,
	[Date_of_Leaving_Secondary_School] [nvarchar](max) NULL,
	[Date_of_Leaving_Junior_College] [nvarchar](max) NULL,
	[Name_of_Secondary_School] [nvarchar](max) NULL,
	[Secondary_School_Village] [nvarchar](50) NULL,
	[Secondary_School_Taluka] [nvarchar](50) NULL,
	[Secondary_School_District] [nvarchar](50) NULL,
	[Secondary_School_State] [nvarchar](50) NULL,
	[Secondary_School_Index_No] [nvarchar](10) NULL,
	[Secondary_School_Udise_No] [nvarchar](20) NULL,
	[Attended_Junior_College_Yes_No] [nvarchar](10) NULL,
	[Name_of_Junior_College] [nvarchar](max) NULL,
	[Junior_College_Village] [nvarchar](50) NULL,
	[Junior_College_District] [nvarchar](50) NULL,
	[Junior_College_State] [nvarchar](50) NULL,
	[Junior_College_Index_No] [nvarchar](10) NULL,
	[Junior_College_Udise_No] [nvarchar](20) NULL,
	[Junior_College_Stream] [nvarchar](10) NULL,
	[FYJC_Passing_Month] [nvarchar](20) NULL,
	[FYJC_Passing_Year] [nvarchar](5) NULL,
	[FYJC_Passing_Status] [nvarchar](10) NULL,
	[Division] [nvarchar](2) NULL,
	[District_of_Form_Submission] [nvarchar](2) NULL,
	[Taluka_of_Form_Submission] [nvarchar](2) NULL,
	[Stream_of_Form] [nvarchar](10) NULL,
	[Medium_of_Form] [nvarchar](10) NULL,
	[College_of_Form_Submission] [nvarchar](max) NULL,
	[College_Index_No] [nvarchar](10) NULL,
	[Declaration_Yes_No] [nvarchar](5) NULL,
	[Name_of_Debarred_Board] [nvarchar](max) NULL,
	[Exam_of_Debar] [nvarchar](max) NULL,
	[Debarred_From_Year] [nvarchar](5) NULL,
	[Debarred_To_Year] [nvarchar](5) NULL,
	[Secondary_School_Certificate_Yes_No] [nvarchar](5) NULL,
	[FYJC_Leaving_Certificate_Yes_No] [nvarchar](5) NULL,
	[School_Leaving_Certificate_Yes_No] [nvarchar](5) NULL,
	[School_Leaving_Duplicate_Certificate_Yes_No] [nvarchar](5) NULL,
	[Unique_ID_Payment] [nvarchar](max) NULL,
	[Selected_Language] [nvarchar](50) NULL,
 CONSTRAINT [PK_Tbl_HSC_Form17] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tbl_HSC_Form17_Final]    Script Date: 01/11/2021 10:58:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tbl_HSC_Form17_Final](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[S_ID] [nvarchar](50) NOT NULL,
	[Ref_ID] [int] NULL,
	[Last_Name] [nvarchar](max) NULL,
	[First_Name] [nvarchar](max) NULL,
	[Father_Husband_Name] [nvarchar](max) NULL,
	[Mother_Name] [nvarchar](max) NULL,
	[Gender] [nvarchar](10) NULL,
	[Residential_Address] [nvarchar](max) NULL,
	[Pincode] [nvarchar](6) NULL,
	[District] [nvarchar](2) NULL,
	[Taluka] [nvarchar](2) NULL,
	[State] [nvarchar](50) NULL,
	[Aadhar_No] [nvarchar](12) NULL,
	[Mobile_No] [nvarchar](10) NULL,
	[Email_ID] [nvarchar](max) NULL,
	[DOB] [nvarchar](max) NULL,
	[Village_of_Birth] [nvarchar](max) NULL,
	[Taluka_of_Birth] [nvarchar](max) NULL,
	[District_of_Birth] [nvarchar](max) NULL,
	[SSC_Passing_Month] [nvarchar](50) NULL,
	[SSC_Passing_Year] [nvarchar](5) NULL,
	[SSC_Division_Board] [nvarchar](50) NULL,
	[Other_SSC_Board] [nvarchar](max) NULL,
	[Date_of_Leaving_Secondary_School] [nvarchar](max) NULL,
	[Date_of_Leaving_Junior_College] [nvarchar](max) NULL,
	[Name_of_Secondary_School] [nvarchar](max) NULL,
	[Secondary_School_Village] [nvarchar](50) NULL,
	[Secondary_School_Taluka] [nvarchar](50) NULL,
	[Secondary_School_District] [nvarchar](50) NULL,
	[Secondary_School_State] [nvarchar](50) NULL,
	[Secondary_School_Index_No] [nvarchar](10) NULL,
	[Secondary_School_Udise_No] [nvarchar](20) NULL,
	[Attended_Junior_College_Yes_No] [nvarchar](10) NULL,
	[Name_of_Junior_College] [nvarchar](max) NULL,
	[Junior_College_Village] [nvarchar](50) NULL,
	[Junior_College_District] [nvarchar](50) NULL,
	[Junior_College_State] [nvarchar](50) NULL,
	[Junior_College_Index_No] [nvarchar](10) NULL,
	[Junior_College_Udise_No] [nvarchar](20) NULL,
	[Junior_College_Stream] [nvarchar](10) NULL,
	[FYJC_Passing_Month] [nvarchar](20) NULL,
	[FYJC_Passing_Year] [nvarchar](5) NULL,
	[FYJC_Passing_Status] [nvarchar](10) NULL,
	[Division] [nvarchar](2) NULL,
	[District_of_Form_Submission] [nvarchar](2) NULL,
	[Taluka_of_Form_Submission] [nvarchar](2) NULL,
	[Stream_of_Form] [nvarchar](10) NULL,
	[Medium_of_Form] [nvarchar](10) NULL,
	[College_of_Form_Submission] [nvarchar](max) NULL,
	[College_Index_No] [nvarchar](10) NULL,
	[Declaration_Yes_No] [nvarchar](5) NULL,
	[Name_of_Debarred_Board] [nvarchar](max) NULL,
	[Exam_of_Debar] [nvarchar](max) NULL,
	[Debarred_From_Year] [nvarchar](5) NULL,
	[Debarred_To_Year] [nvarchar](5) NULL,
	[Secondary_School_Certificate_Yes_No] [nvarchar](5) NULL,
	[FYJC_Leaving_Certificate_Yes_No] [nvarchar](5) NULL,
	[School_Leaving_Certificate_Yes_No] [nvarchar](5) NULL,
	[School_Leaving_Duplicate_Certificate_Yes_No] [nvarchar](5) NULL,
	[Unique_ID_Payment] [nvarchar](max) NULL,
	[Selected_Language] [nvarchar](50) NULL,
	[EC_Status] [nvarchar](10) NULL,
	[EC_Code] [nvarchar](50) NULL,
 CONSTRAINT [PK_Tbl_HSC_Form17_Final] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_Tbl_HSC_Form17_Final] UNIQUE NONCLUSTERED 
(
	[S_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_Tbl_HSC_Form17_Final_1] UNIQUE NONCLUSTERED 
(
	[Ref_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tbl_HSC_Payment]    Script Date: 01/11/2021 10:58:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tbl_HSC_Payment](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[pgRespCode] [nvarchar](max) NULL,
	[PGTxnNo] [nvarchar](max) NULL,
	[SabPaisaTxId] [nvarchar](max) NULL,
	[issuerRefNo] [nvarchar](max) NULL,
	[authIdCode] [nvarchar](max) NULL,
	[amount] [nvarchar](max) NULL,
	[clientTxnId] [nvarchar](50) NOT NULL,
	[firstName] [nvarchar](max) NULL,
	[lastName] [nvarchar](max) NULL,
	[payMode] [nvarchar](max) NULL,
	[email] [nvarchar](max) NULL,
	[mobileNo] [nvarchar](max) NULL,
	[spRespCode] [nvarchar](max) NULL,
	[cid] [nvarchar](max) NULL,
	[bid] [nvarchar](max) NULL,
	[clientCode] [nvarchar](max) NULL,
	[payeeProfile] [nvarchar](max) NULL,
	[transDate] [nvarchar](max) NULL,
	[spRespStatus] [nvarchar](max) NULL,
	[m3] [nvarchar](max) NULL,
	[challanNo] [nvarchar](max) NULL,
	[reMsg] [nvarchar](max) NULL,
	[orgTxnAmount] [nvarchar](max) NULL,
	[programId] [nvarchar](max) NULL,
	[midName] [nvarchar](max) NULL,
	[Add] [nvarchar](max) NULL,
	[param1] [nvarchar](max) NULL,
	[param2] [nvarchar](max) NULL,
	[param3] [nvarchar](max) NULL,
	[param4] [nvarchar](max) NULL,
	[udf5] [nvarchar](max) NULL,
	[udf6] [nvarchar](max) NULL,
	[udf7] [nvarchar](max) NULL,
	[udf8] [nvarchar](max) NULL,
	[udf9] [nvarchar](max) NULL,
	[udf10] [nvarchar](max) NULL,
	[udf11] [nvarchar](max) NULL,
	[udf12] [nvarchar](max) NULL,
	[udf13] [nvarchar](max) NULL,
	[udf14] [nvarchar](max) NULL,
	[udf15] [nvarchar](max) NULL,
	[udf16] [nvarchar](max) NULL,
	[udf17] [nvarchar](max) NULL,
	[udf18] [nvarchar](max) NULL,
	[udf19] [nvarchar](max) NULL,
	[udf20] [nvarchar](max) NULL,
 CONSTRAINT [PK_Tbl_HSC_Payment] PRIMARY KEY CLUSTERED 
(
	[clientTxnId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tbl_HSC_Payment_Error]    Script Date: 01/11/2021 10:58:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tbl_HSC_Payment_Error](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[pgRespCode] [nvarchar](max) NULL,
	[PGTxnNo] [nvarchar](max) NULL,
	[SabPaisaTxId] [nvarchar](max) NULL,
	[issuerRefNo] [nvarchar](max) NULL,
	[authIdCode] [nvarchar](max) NULL,
	[amount] [nvarchar](max) NULL,
	[clientTxnId] [nvarchar](50) NOT NULL,
	[firstName] [nvarchar](max) NULL,
	[lastName] [nvarchar](max) NULL,
	[payMode] [nvarchar](max) NULL,
	[email] [nvarchar](max) NULL,
	[mobileNo] [nvarchar](max) NULL,
	[spRespCode] [nvarchar](max) NULL,
	[cid] [nvarchar](max) NULL,
	[bid] [nvarchar](max) NULL,
	[clientCode] [nvarchar](max) NULL,
	[payeeProfile] [nvarchar](max) NULL,
	[transDate] [nvarchar](max) NULL,
	[spRespStatus] [nvarchar](max) NULL,
	[m3] [nvarchar](max) NULL,
	[challanNo] [nvarchar](max) NULL,
	[reMsg] [nvarchar](max) NULL,
	[orgTxnAmount] [nvarchar](max) NULL,
	[programId] [nvarchar](max) NULL,
	[midName] [nvarchar](max) NULL,
	[Add] [nvarchar](max) NULL,
	[param1] [nvarchar](max) NULL,
	[param2] [nvarchar](max) NULL,
	[param3] [nvarchar](max) NULL,
	[param4] [nvarchar](max) NULL,
	[udf5] [nvarchar](max) NULL,
	[udf6] [nvarchar](max) NULL,
	[udf7] [nvarchar](max) NULL,
	[udf8] [nvarchar](max) NULL,
	[udf9] [nvarchar](max) NULL,
	[udf10] [nvarchar](max) NULL,
	[udf11] [nvarchar](max) NULL,
	[udf12] [nvarchar](max) NULL,
	[udf13] [nvarchar](max) NULL,
	[udf14] [nvarchar](max) NULL,
	[udf15] [nvarchar](max) NULL,
	[udf16] [nvarchar](max) NULL,
	[udf17] [nvarchar](max) NULL,
	[udf18] [nvarchar](max) NULL,
	[udf19] [nvarchar](max) NULL,
	[udf20] [nvarchar](max) NULL,
 CONSTRAINT [PK_Tbl_HSC_Payment_Error] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tbl_Login_Credentials]    Script Date: 01/11/2021 10:58:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tbl_Login_Credentials](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Division_Name] [nvarchar](50) NULL,
	[Username] [nvarchar](50) NULL,
	[Password] [nvarchar](50) NULL,
 CONSTRAINT [PK_Tbl_Login_Credentials] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tbl_Site_Status]    Script Date: 01/11/2021 10:58:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tbl_Site_Status](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Active_Status] [int] NULL,
	[Late_Fee_Date] [date] NULL,
	[Super_Late_Fee_Date] [date] NULL,
 CONSTRAINT [PK_Tbl_Site_Status] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tbl_SSC_Form17]    Script Date: 01/11/2021 10:58:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tbl_SSC_Form17](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Last_Name] [nvarchar](max) NULL,
	[First_Name] [nvarchar](max) NULL,
	[Father_Husband_Name] [nvarchar](max) NULL,
	[Mother_Name] [nvarchar](max) NULL,
	[Gender] [nvarchar](10) NULL,
	[Residential_Address] [nvarchar](max) NULL,
	[Pincode] [nvarchar](6) NULL,
	[District] [nvarchar](2) NULL,
	[Taluka] [nvarchar](2) NULL,
	[State] [nvarchar](50) NULL,
	[Aadhar_No] [nvarchar](12) NULL,
	[Mobile_No] [nvarchar](10) NULL,
	[Email_ID] [nvarchar](max) NULL,
	[DOB] [nvarchar](max) NULL,
	[Village_of_Birth] [nvarchar](max) NULL,
	[Taluka_of_Birth] [nvarchar](max) NULL,
	[District_of_Birth] [nvarchar](max) NULL,
	[Name_of_Secondary_School] [nvarchar](max) NULL,
	[Secondary_School_Village] [nvarchar](50) NULL,
	[Secondary_School_Taluka] [nvarchar](50) NULL,
	[Secondary_School_District] [nvarchar](50) NULL,
	[Secondary_School_State] [nvarchar](50) NULL,
	[Secondary_School_Udise_No] [nvarchar](20) NULL,
	[Secondary_School_Pincode] [nvarchar](10) NULL,
	[Whether_Handicap] [nvarchar](5) NULL,
	[Handicap_Category] [nvarchar](50) NULL,
	[Date_of_Leaving_Secondary_School] [nvarchar](max) NULL,
	[Last_Studied_Standard] [nvarchar](2) NULL,
	[Status_of_Last_Studied_Standard] [nvarchar](15) NULL,
	[Division] [nvarchar](2) NULL,
	[District_of_Form_Submission] [nvarchar](2) NULL,
	[Taluka_of_Form_Submission] [nvarchar](2) NULL,
	[Medium_of_Form] [nvarchar](10) NULL,
	[School_of_Form_Submission] [nvarchar](max) NULL,
	[Index_No_of_School] [nvarchar](10) NULL,
	[Declaration_Yes_No] [nvarchar](5) NULL,
	[Name_of_Debarred_Board] [nvarchar](max) NULL,
	[Exam_of_Debar] [nvarchar](max) NULL,
	[Debarred_From_Year] [nvarchar](5) NULL,
	[Debarred_To_Year] [nvarchar](5) NULL,
	[Leaving_Certificate_Yes_No] [nvarchar](5) NULL,
	[Duplicate_Leaving_Certificate_Yes_No] [nvarchar](5) NULL,
	[Unique_ID_Payment] [nvarchar](max) NULL,
	[Selected_Language] [nvarchar](50) NULL,
 CONSTRAINT [PK_Tbl_SSC_Form17] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tbl_SSC_Form17_Final]    Script Date: 01/11/2021 10:58:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tbl_SSC_Form17_Final](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[S_ID] [nvarchar](50) NULL,
	[Ref_ID] [int] NULL,
	[Last_Name] [nvarchar](max) NULL,
	[First_Name] [nvarchar](max) NULL,
	[Father_Husband_Name] [nvarchar](max) NULL,
	[Mother_Name] [nvarchar](max) NULL,
	[Gender] [nvarchar](10) NULL,
	[Residential_Address] [nvarchar](max) NULL,
	[Pincode] [nvarchar](6) NULL,
	[District] [nvarchar](2) NULL,
	[Taluka] [nvarchar](2) NULL,
	[State] [nvarchar](50) NULL,
	[Aadhar_No] [nvarchar](12) NULL,
	[Mobile_No] [nvarchar](10) NULL,
	[Email_ID] [nvarchar](max) NULL,
	[DOB] [nvarchar](max) NULL,
	[Village_of_Birth] [nvarchar](max) NULL,
	[Taluka_of_Birth] [nvarchar](max) NULL,
	[District_of_Birth] [nvarchar](max) NULL,
	[Name_of_Secondary_School] [nvarchar](max) NULL,
	[Secondary_School_Village] [nvarchar](50) NULL,
	[Secondary_School_Taluka] [nvarchar](50) NULL,
	[Secondary_School_District] [nvarchar](50) NULL,
	[Secondary_School_State] [nvarchar](50) NULL,
	[Secondary_School_Udise_No] [nvarchar](20) NULL,
	[Secondary_School_Pincode] [nvarchar](10) NULL,
	[Whether_Handicap] [nvarchar](5) NULL,
	[Handicap_Category] [nvarchar](50) NULL,
	[Date_of_Leaving_Secondary_School] [nvarchar](max) NULL,
	[Last_Studied_Standard] [nvarchar](2) NULL,
	[Status_of_Last_Studied_Standard] [nvarchar](15) NULL,
	[Division] [nvarchar](2) NULL,
	[District_of_Form_Submission] [nvarchar](2) NULL,
	[Taluka_of_Form_Submission] [nvarchar](2) NULL,
	[Medium_of_Form] [nvarchar](10) NULL,
	[School_of_Form_Submission] [nvarchar](max) NULL,
	[Index_No_of_School] [nvarchar](10) NULL,
	[Declaration_Yes_No] [nvarchar](5) NULL,
	[Name_of_Debarred_Board] [nvarchar](max) NULL,
	[Exam_of_Debar] [nvarchar](max) NULL,
	[Debarred_From_Year] [nvarchar](5) NULL,
	[Debarred_To_Year] [nvarchar](5) NULL,
	[Leaving_Certificate_Yes_No] [nvarchar](5) NULL,
	[Duplicate_Leaving_Certificate_Yes_No] [nvarchar](5) NULL,
	[Unique_ID_Payment] [nvarchar](max) NULL,
	[Selected_Language] [nvarchar](50) NULL,
	[EC_Status] [nvarchar](10) NULL,
	[EC_Code] [nvarchar](50) NULL,
 CONSTRAINT [PK_Tbl_SSC_Form17_Final] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_Tbl_SSC_Form17_Final] UNIQUE NONCLUSTERED 
(
	[S_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_Tbl_SSC_Form17_Final_1] UNIQUE NONCLUSTERED 
(
	[Ref_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tbl_SSC_Payment]    Script Date: 01/11/2021 10:58:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tbl_SSC_Payment](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[pgRespCode] [nvarchar](max) NULL,
	[PGTxnNo] [nvarchar](max) NULL,
	[SabPaisaTxId] [nvarchar](max) NULL,
	[issuerRefNo] [nvarchar](max) NULL,
	[authIdCode] [nvarchar](max) NULL,
	[amount] [nvarchar](max) NULL,
	[clientTxnId] [nvarchar](50) NOT NULL,
	[firstName] [nvarchar](max) NULL,
	[lastName] [nvarchar](max) NULL,
	[payMode] [nvarchar](max) NULL,
	[email] [nvarchar](max) NULL,
	[mobileNo] [nvarchar](max) NULL,
	[spRespCode] [nvarchar](max) NULL,
	[cid] [nvarchar](max) NULL,
	[bid] [nvarchar](max) NULL,
	[clientCode] [nvarchar](max) NULL,
	[payeeProfile] [nvarchar](max) NULL,
	[transDate] [nvarchar](max) NULL,
	[spRespStatus] [nvarchar](max) NULL,
	[m3] [nvarchar](max) NULL,
	[challanNo] [nvarchar](max) NULL,
	[reMsg] [nvarchar](max) NULL,
	[orgTxnAmount] [nvarchar](max) NULL,
	[programId] [nvarchar](max) NULL,
	[midName] [nvarchar](max) NULL,
	[Add] [nvarchar](max) NULL,
	[param1] [nvarchar](max) NULL,
	[param2] [nvarchar](max) NULL,
	[param3] [nvarchar](max) NULL,
	[param4] [nvarchar](max) NULL,
	[udf5] [nvarchar](max) NULL,
	[udf6] [nvarchar](max) NULL,
	[udf7] [nvarchar](max) NULL,
	[udf8] [nvarchar](max) NULL,
	[udf9] [nvarchar](max) NULL,
	[udf10] [nvarchar](max) NULL,
	[udf11] [nvarchar](max) NULL,
	[udf12] [nvarchar](max) NULL,
	[udf13] [nvarchar](max) NULL,
	[udf14] [nvarchar](max) NULL,
	[udf15] [nvarchar](max) NULL,
	[udf16] [nvarchar](max) NULL,
	[udf17] [nvarchar](max) NULL,
	[udf18] [nvarchar](max) NULL,
	[udf19] [nvarchar](max) NULL,
	[udf20] [nvarchar](max) NULL,
 CONSTRAINT [PK_Tbl_SSC_Payment] PRIMARY KEY CLUSTERED 
(
	[clientTxnId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tbl_SSC_Payment_Error]    Script Date: 01/11/2021 10:58:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tbl_SSC_Payment_Error](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[pgRespCode] [nvarchar](max) NULL,
	[PGTxnNo] [nvarchar](max) NULL,
	[SabPaisaTxId] [nvarchar](max) NULL,
	[issuerRefNo] [nvarchar](max) NULL,
	[authIdCode] [nvarchar](max) NULL,
	[amount] [nvarchar](max) NULL,
	[clientTxnId] [nvarchar](50) NOT NULL,
	[firstName] [nvarchar](max) NULL,
	[lastName] [nvarchar](max) NULL,
	[payMode] [nvarchar](max) NULL,
	[email] [nvarchar](max) NULL,
	[mobileNo] [nvarchar](max) NULL,
	[spRespCode] [nvarchar](max) NULL,
	[cid] [nvarchar](max) NULL,
	[bid] [nvarchar](max) NULL,
	[clientCode] [nvarchar](max) NULL,
	[payeeProfile] [nvarchar](max) NULL,
	[transDate] [nvarchar](max) NULL,
	[spRespStatus] [nvarchar](max) NULL,
	[m3] [nvarchar](max) NULL,
	[challanNo] [nvarchar](max) NULL,
	[reMsg] [nvarchar](max) NULL,
	[orgTxnAmount] [nvarchar](max) NULL,
	[programId] [nvarchar](max) NULL,
	[midName] [nvarchar](max) NULL,
	[Add] [nvarchar](max) NULL,
	[param1] [nvarchar](max) NULL,
	[param2] [nvarchar](max) NULL,
	[param3] [nvarchar](max) NULL,
	[param4] [nvarchar](max) NULL,
	[udf5] [nvarchar](max) NULL,
	[udf6] [nvarchar](max) NULL,
	[udf7] [nvarchar](max) NULL,
	[udf8] [nvarchar](max) NULL,
	[udf9] [nvarchar](max) NULL,
	[udf10] [nvarchar](max) NULL,
	[udf11] [nvarchar](max) NULL,
	[udf12] [nvarchar](max) NULL,
	[udf13] [nvarchar](max) NULL,
	[udf14] [nvarchar](max) NULL,
	[udf15] [nvarchar](max) NULL,
	[udf16] [nvarchar](max) NULL,
	[udf17] [nvarchar](max) NULL,
	[udf18] [nvarchar](max) NULL,
	[udf19] [nvarchar](max) NULL,
	[udf20] [nvarchar](max) NULL,
 CONSTRAINT [PK_Tbl_SSC_Payment_Error] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tbl_Unique_ID_Payment]    Script Date: 01/11/2021 10:58:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tbl_Unique_ID_Payment](
	[Unique_ID_Payment] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Unique_ID_Payment] PRIMARY KEY CLUSTERED 
(
	[Unique_ID_Payment] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tbl_Update_Old_College]    Script Date: 01/11/2021 10:58:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tbl_Update_Old_College](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Form_No] [nvarchar](50) NULL,
	[Old_College] [nvarchar](50) NULL,
	[Updated_College] [nvarchar](50) NULL,
	[Email_ID] [nvarchar](50) NULL,
 CONSTRAINT [PK_Tbl_Update_Old_College] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Tbl_HSC_Form17_Final]  WITH CHECK ADD  CONSTRAINT [FK_Tbl_HSC_Form17_Final_Tbl_HSC_Form17] FOREIGN KEY([Ref_ID])
REFERENCES [dbo].[Tbl_HSC_Form17] ([ID])
GO
ALTER TABLE [dbo].[Tbl_HSC_Form17_Final] CHECK CONSTRAINT [FK_Tbl_HSC_Form17_Final_Tbl_HSC_Form17]
GO
ALTER TABLE [dbo].[Tbl_SSC_Form17_Final]  WITH CHECK ADD  CONSTRAINT [FK_Tbl_SSC_Form17_Final_Tbl_SSC_Form17] FOREIGN KEY([Ref_ID])
REFERENCES [dbo].[Tbl_SSC_Form17] ([ID])
GO
ALTER TABLE [dbo].[Tbl_SSC_Form17_Final] CHECK CONSTRAINT [FK_Tbl_SSC_Form17_Final_Tbl_SSC_Form17]
GO
ALTER TABLE [dbo].[Tbl_SSC_Payment]  WITH CHECK ADD  CONSTRAINT [FK_Tbl_SSC_Payment_Tbl_SSC_Payment] FOREIGN KEY([clientTxnId])
REFERENCES [dbo].[Tbl_SSC_Payment] ([clientTxnId])
GO
ALTER TABLE [dbo].[Tbl_SSC_Payment] CHECK CONSTRAINT [FK_Tbl_SSC_Payment_Tbl_SSC_Payment]
GO
/****** Object:  StoredProcedure [dbo].[INSERT_DATA_HSC]    Script Date: 01/11/2021 10:58:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[INSERT_DATA_HSC]

@s_id nvarchar(50),
@div nvarchar(max),
@RESULT int OUT
as 
BEGIN
	SET NOCOUNT ON;
		DECLARE @SCOUNT int
		DECLARE @SID nvarchar(50)
		Set @SCOUNT= (select (Count(*)+1) from Tbl_HSC_Form17_Final where Division=@div)		
		Set @SID='H'+ CAST(@div AS varchar)   + CAST((10000+@SCOUNT) AS varchar)

		SET @RESULT = 0	
		insert into Tbl_HSC_Form17_Final (S_ID,Ref_ID,Last_Name,	First_Name,	Father_Husband_Name,	Mother_Name,	Gender,	Residential_Address,	Pincode,	District,	Taluka,	State,	Aadhar_No,	Mobile_No,	Email_ID,	DOB,	Village_of_Birth,	Taluka_of_Birth,	District_of_Birth,	SSC_Passing_Month,	SSC_Passing_Year,	SSC_Division_Board,	Other_SSC_Board,	Date_of_Leaving_Secondary_School,	Date_of_Leaving_Junior_College,	Name_of_Secondary_School,	Secondary_School_Village,	Secondary_School_Taluka,	Secondary_School_District,	Secondary_School_State,	Secondary_School_Index_No,	Secondary_School_Udise_No,	Attended_Junior_College_Yes_No,	Name_of_Junior_College,	Junior_College_Village,	Junior_College_District,	Junior_College_State,	Junior_College_Index_No,	Junior_College_Udise_No,	Junior_College_Stream,	FYJC_Passing_Month,	FYJC_Passing_Year,	FYJC_Passing_Status,	District_of_Form_Submission,	Taluka_of_Form_Submission,	Stream_of_Form,	Medium_of_Form,	College_of_Form_Submission,	College_Index_No,	Declaration_Yes_No,	Name_of_Debarred_Board,	Exam_of_Debar,	Debarred_From_Year,	Debarred_To_Year,	Secondary_School_Certificate_Yes_No,	FYJC_Leaving_Certificate_Yes_No,	School_Leaving_Certificate_Yes_No,	School_Leaving_Duplicate_Certificate_Yes_No,	Unique_ID_Payment,	Selected_Language,Division)
								select    @SID,ID,	Last_Name,	First_Name,	Father_Husband_Name,	Mother_Name,	Gender,	Residential_Address,	Pincode,	District,	Taluka,	State,	Aadhar_No,	Mobile_No,	Email_ID,	DOB,	Village_of_Birth,	Taluka_of_Birth,	District_of_Birth,	SSC_Passing_Month,	SSC_Passing_Year,	SSC_Division_Board,	Other_SSC_Board,	Date_of_Leaving_Secondary_School,	Date_of_Leaving_Junior_College,	Name_of_Secondary_School,	Secondary_School_Village,	Secondary_School_Taluka,	Secondary_School_District,	Secondary_School_State,	Secondary_School_Index_No,	Secondary_School_Udise_No,	Attended_Junior_College_Yes_No,	Name_of_Junior_College,	Junior_College_Village,	Junior_College_District,	Junior_College_State,	Junior_College_Index_No,	Junior_College_Udise_No,	Junior_College_Stream,	FYJC_Passing_Month,	FYJC_Passing_Year,	FYJC_Passing_Status,	District_of_Form_Submission,	Taluka_of_Form_Submission,	Stream_of_Form,	Medium_of_Form,	College_of_Form_Submission,	College_Index_No,	Declaration_Yes_No,	Name_of_Debarred_Board,	Exam_of_Debar,	Debarred_From_Year,	Debarred_To_Year,	Secondary_School_Certificate_Yes_No,	FYJC_Leaving_Certificate_Yes_No,	School_Leaving_Certificate_Yes_No,	School_Leaving_Duplicate_Certificate_Yes_No,	Unique_ID_Payment,	Selected_Language,@div
								from Tbl_HSC_Form17 where ID=@s_id
		
		SET @RESULT =SCOPE_IDENTITY()

END







GO
/****** Object:  StoredProcedure [dbo].[INSERT_DATA_SSC]    Script Date: 01/11/2021 10:58:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[INSERT_DATA_SSC]

@s_id nvarchar(50),
@div nvarchar(max),
@RESULT int OUT
as 
BEGIN
	SET NOCOUNT ON;
		DECLARE @SCOUNT int
		DECLARE @SID nvarchar(50)
		Set @SCOUNT= (select (Count(*)+1) from Tbl_SSC_Form17_Final where Division=@div)		
		Set @SID='S'+ CAST(@div AS varchar)   + CAST((10000+@SCOUNT) AS varchar)

		SET @RESULT = 0	
		insert into Tbl_SSC_Form17_Final (S_ID,	Ref_ID,	Last_Name,	First_Name,	Father_Husband_Name,	Mother_Name,	Gender,	Residential_Address,	Pincode,	District,	Taluka,	State,	Aadhar_No,	Mobile_No,	Email_ID,	DOB,	Village_of_Birth,	Taluka_of_Birth,	District_of_Birth,	Name_of_Secondary_School,	Secondary_School_Village,	Secondary_School_Taluka,	Secondary_School_District,	Secondary_School_State,	Secondary_School_Udise_No,	Secondary_School_Pincode,	Whether_Handicap,	Handicap_Category,	Date_of_Leaving_Secondary_School,	Last_Studied_Standard,	Status_of_Last_Studied_Standard,	Division,	District_of_Form_Submission,	Taluka_of_Form_Submission,	Medium_of_Form,	School_of_Form_Submission,	Index_No_of_School,	Declaration_Yes_No,	Name_of_Debarred_Board,	Exam_of_Debar,	Debarred_From_Year,	Debarred_To_Year,	Leaving_Certificate_Yes_No,	Duplicate_Leaving_Certificate_Yes_No,	Unique_ID_Payment,	Selected_Language)
								select    @SID,ID,	Last_Name,	First_Name,	Father_Husband_Name,	Mother_Name,	Gender,	Residential_Address,	Pincode,	District,	Taluka,	State,	Aadhar_No,	Mobile_No,	Email_ID,	DOB,	Village_of_Birth,	Taluka_of_Birth,	District_of_Birth,	Name_of_Secondary_School,	Secondary_School_Village,	Secondary_School_Taluka,	Secondary_School_District,	Secondary_School_State,	Secondary_School_Udise_No,	Secondary_School_Pincode,	Whether_Handicap,	Handicap_Category,	Date_of_Leaving_Secondary_School,	Last_Studied_Standard,	Status_of_Last_Studied_Standard,	Division,	District_of_Form_Submission,	Taluka_of_Form_Submission,	Medium_of_Form,	School_of_Form_Submission,	Index_No_of_School,	Declaration_Yes_No,	Name_of_Debarred_Board,	Exam_of_Debar,	Debarred_From_Year,	Debarred_To_Year,	Leaving_Certificate_Yes_No,	Duplicate_Leaving_Certificate_Yes_No,	Unique_ID_Payment,	Selected_Language
								from Tbl_SSC_Form17 where ID=@s_id
		
		SET @RESULT =SCOPE_IDENTITY()

END







GO
