USE [HPDQ]
GO
/****** Object:  StoredProcedure [dbo].[Chucvu_delete]    Script Date: 31/07/2020 4:34:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Chucvu_delete]
@MaCV int
AS
BEGIN
	delete from ChucVu where MaCV=@MaCV
END

GO
/****** Object:  StoredProcedure [dbo].[Chucvu_insert]    Script Date: 31/07/2020 4:34:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Chucvu_insert]
@TenCV nvarchar(50)
AS
BEGIN
	insert into ChucVu(TenCV) values(@TenCV)
END

GO
/****** Object:  StoredProcedure [dbo].[Chucvu_update]    Script Date: 31/07/2020 4:34:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Chucvu_update]
@MaCV int,
@TenCV nvarchar(50)
AS
BEGIN
	update ChucVu set TenCV=@TenCV where MaCV=@MaCV
END

GO
/****** Object:  StoredProcedure [dbo].[Dangky_delete]    Script Date: 31/07/2020 4:34:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Dangky_delete]
@TenDangNhap nvarchar(50)
AS
BEGIN
	delete from DangKy where TenDangNhap=@TenDangNhap
END

GO
/****** Object:  StoredProcedure [dbo].[Dangky_insert]    Script Date: 31/07/2020 4:34:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Dangky_insert]
@TenDangNhap nvarchar(50),
@MatKhau nvarchar(50),
@MaNV nchar(9),
@MaQuyen int,
@NgayCap date,
@TrangThai int
AS
BEGIN
	insert into DangKy(TenDangNhap,MatKhau,MaNV,MaQuyen,NgayCap,TrangThai) values (@TenDangNhap,@MatKhau,@MaNV,@MaQuyen,@NgayCap,@TrangThai)
END

GO
/****** Object:  StoredProcedure [dbo].[Dangky_update]    Script Date: 31/07/2020 4:34:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Dangky_update]
@TenDangNhap nvarchar(50),
@MaNV nchar(9),
@MaQuyen int,
@NgayCap date,
@TrangThai int
AS
BEGIN
	update DangKy set MaNV=@MaNV,MaQuyen=@MaQuyen,NgayCap=@NgayCap,TrangThai=@TrangThai where TenDangNhap=@TenDangNhap
END

GO
/****** Object:  StoredProcedure [dbo].[Dangky_update_password]    Script Date: 31/07/2020 4:34:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Dangky_update_password]
@TenDangNhap nvarchar(50),
@MatKhau nvarchar(50)
AS
BEGIN
	update DangKy set MatKhau=@MatKhau where TenDangNhap=@TenDangNhap
END

GO
/****** Object:  StoredProcedure [dbo].[Menu_delete]    Script Date: 31/07/2020 4:34:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Menu_delete]
@MaMenu int
AS
BEGIN
	delete from Menu where MaMenu=@MaMenu
END

GO
/****** Object:  StoredProcedure [dbo].[Menu_insert]    Script Date: 31/07/2020 4:34:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Menu_insert]
@TenMenu nvarchar(50),
@LienKet nvarchar(50),
@MaMenuCha int,
@ThuTu int,
@TrangThai int
AS
BEGIN
	insert into Menu(TenMenu,LienKet,MaMenuCha,ThuTu,TrangThai) values(@TenMenu,@LienKet,@MaMenuCha,@ThuTu,@TrangThai)
END

GO
/****** Object:  StoredProcedure [dbo].[Menu_update]    Script Date: 31/07/2020 4:34:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Menu_update]
@MaMenu int,
@TenMenu nvarchar(50),
@LienKet nvarchar(50),
@MaMenuCha int,
@ThuTu int,
@TrangThai int
AS
BEGIN
	update Menu set TenMenu=@TenMenu,LienKet=@LienKet,MaMenuCha=@MaMenuCha,ThuTu=@ThuTu,TrangThai=@TrangThai where MaMenu=@MaMenu
END

GO
/****** Object:  StoredProcedure [dbo].[Nhanvien_delete]    Script Date: 31/07/2020 4:34:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Nhanvien_delete]
@MaNV nchar(9)
AS
BEGIN
	delete from NhanVien where MaNV=@MaNV
END

GO
/****** Object:  StoredProcedure [dbo].[Nhanvien_insert]    Script Date: 31/07/2020 4:34:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Nhanvien_insert]
@MaNV nchar(9),
@TenNV nvarchar(30),
@MaCV int,
@MaPB int,
@DiaChi nvarchar(120),
@SDT nchar(11),
@SoMayLe int,
@GioiTinh nchar(10),
@Email nvarchar(50),
@NgayVaoLam date
AS
BEGIN
	insert into NhanVien(MaNV,TenNV,MaCV,MaPB,DiaChi,SDT,SoMayLe,GioiTinh,Email,NgayVaoLam) values(@MaNV,@TenNV,@MaCV,@MaPB,@DiaChi,@SDT,@SoMayLe,@GioiTinh,@Email,@NgayVaoLam)
END

GO
/****** Object:  StoredProcedure [dbo].[Nhanvien_update]    Script Date: 31/07/2020 4:34:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Nhanvien_update]
@MaNV nchar(9),
@TenNV nvarchar(30),
@MaCV int,
@MaPB int,
@DiaChi nvarchar(120),
@SDT nchar(11),
@SoMayLe int,
@GioiTinh nchar(10),
@Email nvarchar(50),
@NgayVaoLam date
AS
BEGIN
	update NhanVien set TenNV=@TenNV,MaCV=@MaCV,MaPB=@MaPB,DiaChi=@DiaChi,SDT=@SDT,SoMayLe=@SoMayLe,GioiTinh=@GioiTinh,Email=@Email,NgayVaoLam=@NgayVaoLam where MaNV=@MaNV
END

GO
/****** Object:  StoredProcedure [dbo].[Phanmem_delete]    Script Date: 31/07/2020 4:34:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Phanmem_delete]
@MaPM int
AS
BEGIN
	delete from PhanMem where MaPM=@MaPM
END

GO
/****** Object:  StoredProcedure [dbo].[Phanmem_insert]    Script Date: 31/07/2020 4:34:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Phanmem_insert]
@TenPM nvarchar(30)
AS
BEGIN
	insert into PhanMem(TenPM) values (@TenPM)
END

GO
/****** Object:  StoredProcedure [dbo].[Phanmem_update]    Script Date: 31/07/2020 4:34:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Phanmem_update]
@MaPM int,
@TenPM nvarchar(30)
AS
BEGIN
	update PhanMem set TenPM=@TenPM where MaPM=@MaPM
END

GO
/****** Object:  StoredProcedure [dbo].[Phanquyen_delete]    Script Date: 31/07/2020 4:34:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Phanquyen_delete]
@TenDangNhap nvarchar(50),
@MaMenu int,
@MaTT int
AS
BEGIN
	delete from PhanQuyen where TenDangNhap=@TenDangNhap and MaMenu=@MaMenu and MaTT=@MaTT
END

GO
/****** Object:  StoredProcedure [dbo].[Phanquyen_insert]    Script Date: 31/07/2020 4:34:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Phanquyen_insert]
@TenDangNhap nvarchar(50),
@MaMenu int,
@MaTT int,
@GiaTri int
AS
BEGIN
	insert into PhanQuyen(TenDangNhap,MaMenu,MaTT,GiaTri) values(@TenDangNhap,@MaMenu,@MaTT,@GiaTri)
END

GO
/****** Object:  StoredProcedure [dbo].[Phanquyen_update]    Script Date: 31/07/2020 4:34:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Phanquyen_update]
@TenDangNhap nvarchar(50),
@MaMenu int,
@MaTT int,
@GiaTri int
AS
BEGIN
	update PhanQuyen set GiaTri=@GiaTri where TenDangNhap=@TenDangNhap and MaMenu=@MaMenu and MaTT=@MaTT
END

GO
/****** Object:  StoredProcedure [dbo].[Phongban_delete]    Script Date: 31/07/2020 4:34:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Phongban_delete]
@MaPB int
AS
BEGIN
	delete from PhongBan where MaPB=@MaPB
END

GO
/****** Object:  StoredProcedure [dbo].[Phongban_insert]    Script Date: 31/07/2020 4:34:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Phongban_insert]
@TenPB nvarchar(50)
AS
BEGIN
	insert into PhongBan(TenPB) values(@TenPB)
END

GO
/****** Object:  StoredProcedure [dbo].[Phongban_update]    Script Date: 31/07/2020 4:34:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Phongban_update]
@MaPB int,
@TenPB nvarchar(50)
AS
BEGIN
	update PhongBan set TenPB=@TenPB where MaPB=@MaPB
END

GO
/****** Object:  StoredProcedure [dbo].[Quyendangnhap_delete]    Script Date: 31/07/2020 4:34:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Quyendangnhap_delete]
@MaQuyen int
AS
BEGIN
	delete from QuyenDangNhap where MaQuyen=@MaQuyen
END

GO
/****** Object:  StoredProcedure [dbo].[Quyendangnhap_insert]    Script Date: 31/07/2020 4:34:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Quyendangnhap_insert]
@TenQuyen nchar(10)
AS
BEGIN
	insert into QuyenDangNhap(TenQuyen) values(@TenQuyen)
END

GO
/****** Object:  StoredProcedure [dbo].[Quyendangnhap_update]    Script Date: 31/07/2020 4:34:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Quyendangnhap_update]
@MaQuyen int,
@TenQuyen nchar(10)
AS
BEGIN
	update QuyenDangNhap set TenQuyen=@TenQuyen where MaQuyen=@MaQuyen
END

GO
/****** Object:  StoredProcedure [dbo].[Taikhoan_delete]    Script Date: 31/07/2020 4:34:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Taikhoan_delete]
@TKID int
AS
BEGIN
	delete from TaiKhoan where TKID=@TKID
END

GO
/****** Object:  StoredProcedure [dbo].[Taikhoan_insert]    Script Date: 31/07/2020 4:34:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Taikhoan_insert]
@MaNV nchar(9),
@MaPM int,
@TenTK nvarchar(50),
@NgayCap date,
@TrangThai int,
@GhiChu nvarchar(100)
AS
BEGIN
	insert into TaiKhoan(MaNV,MaPM,TenTK,NgayCap,TrangThai,GhiChu) values(@MaNV,@MaPM,@TenTK,@NgayCap,@TrangThai,@GhiChu)
END

GO
/****** Object:  StoredProcedure [dbo].[Taikhoan_update]    Script Date: 31/07/2020 4:34:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Taikhoan_update]
@TKID int,
@MaNV nchar(9),
@MaPM int,
@TenTK nvarchar(50),
@NgayCap date,
@TrangThai int,
@GhiChu nvarchar(100)
AS
BEGIN
	update TaiKhoan set MaNV=@MaNV,MaPM=@MaPM,TenTK=@TenTK,NgayCap=@NgayCap,TrangThai=@TrangThai,GhiChu=@GhiChu where TKID=@TKID
END

GO
/****** Object:  StoredProcedure [dbo].[Thaotac_delete]    Script Date: 31/07/2020 4:34:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Thaotac_delete]
@MaTT int
AS
BEGIN
	delete from ThaoTac where MaTT=@MaTT
END

GO
/****** Object:  StoredProcedure [dbo].[Thaotac_insert]    Script Date: 31/07/2020 4:34:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Thaotac_insert]
@TenTT nvarchar(20)
AS
BEGIN
	insert into ThaoTac(TenTT) values(@TenTT)
END

GO
/****** Object:  StoredProcedure [dbo].[Thaotac_update]    Script Date: 31/07/2020 4:34:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Thaotac_update]
@MaTT int,
@TenTT nvarchar(20)
AS
BEGIN
	update ThaoTac set TenTT=@TenTT where MaTT=@MaTT
END

GO
/****** Object:  StoredProcedure [dbo].[thongtin_danhba]    Script Date: 31/07/2020 4:34:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[thongtin_danhba]

AS
BEGIN
	select NhanVien.MaNV,NhanVien.TenNV,ChucVu.TenCV,PhongBan.TenPB,NhanVien.SoMayLe,NhanVien.SDT,NhanVien.Email from NhanVien inner join ChucVu on NhanVien.MaCV=ChucVu.MaCV inner join PhongBan on NhanVien.MaPB=PhongBan.MaPB
END

GO
/****** Object:  StoredProcedure [dbo].[thongtin_taikhoan]    Script Date: 31/07/2020 4:34:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[thongtin_taikhoan]
AS
BEGIN
	select NhanVien.MaNV,NhanVien.TenNV,PhongBan.TenPB,TaiKhoan.TenTK,PhanMem.TenPM,TaiKhoan.NgayCap,TinhTrang.TenTinhTrang from TaiKhoan inner join NhanVien on TaiKhoan.MaNV=NhanVien.MaCV inner join PhongBan on NhanVien.MaPB=PhongBan.MaPB inner join PhanMem on TaiKhoan.MaPM=PhanMem.MaPM inner join TinhTrang on TaiKhoan.MaTinhTrang=TinhTrang.MaTinhTrang
END

GO
/****** Object:  Table [dbo].[ChucVu]    Script Date: 31/07/2020 4:34:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ChucVu](
	[MaCV] [int] IDENTITY(1,1) NOT NULL,
	[TenCV] [nvarchar](50) NULL,
 CONSTRAINT [PK_ChucVu] PRIMARY KEY CLUSTERED 
(
	[MaCV] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[DangKy]    Script Date: 31/07/2020 4:34:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DangKy](
	[TenDangNhap] [nvarchar](50) NOT NULL,
	[MatKhau] [nvarchar](50) NULL,
	[MaNV] [nchar](9) NULL,
	[MaQuyen] [int] NULL,
	[NgayCap] [date] NULL,
	[TrangThai] [int] NULL,
 CONSTRAINT [PK_DangKy] PRIMARY KEY CLUSTERED 
(
	[TenDangNhap] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[GioiTinh]    Script Date: 31/07/2020 4:34:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GioiTinh](
	[MaGT] [int] IDENTITY(1,1) NOT NULL,
	[TenGT] [nchar](10) NULL,
 CONSTRAINT [PK_GioiTinh] PRIMARY KEY CLUSTERED 
(
	[MaGT] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Menu]    Script Date: 31/07/2020 4:34:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Menu](
	[MaMenu] [int] IDENTITY(1,1) NOT NULL,
	[TenMenu] [nvarchar](50) NULL,
	[LienKet] [nvarchar](50) NULL,
	[MaMenuCha] [int] NULL,
	[ThuTu] [int] NULL,
	[TrangThai] [int] NULL,
	[Icon] [nvarchar](20) NULL,
 CONSTRAINT [PK_Menu] PRIMARY KEY CLUSTERED 
(
	[MaMenu] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[NhanVien]    Script Date: 31/07/2020 4:34:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NhanVien](
	[MaNV] [nchar](9) NOT NULL,
	[TenNV] [nvarchar](30) NULL,
	[MaCV] [int] NULL,
	[MaPB] [int] NULL,
	[DiaChi] [nvarchar](120) NULL,
	[SDT] [nchar](11) NULL,
	[SoMayLe] [int] NULL,
	[MaGT] [int] NULL,
	[Email] [nvarchar](50) NULL,
	[NgayVaoLam] [date] NULL,
 CONSTRAINT [PK_NhanVien] PRIMARY KEY CLUSTERED 
(
	[MaNV] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[PhanMem]    Script Date: 31/07/2020 4:34:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PhanMem](
	[MaPM] [int] IDENTITY(1,1) NOT NULL,
	[TenPM] [nvarchar](30) NULL,
 CONSTRAINT [PK_PhanMem] PRIMARY KEY CLUSTERED 
(
	[MaPM] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[PhanQuyen]    Script Date: 31/07/2020 4:34:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PhanQuyen](
	[TenDangNhap] [nvarchar](50) NOT NULL,
	[MaMenu] [int] NOT NULL,
	[MaTT] [int] NOT NULL,
	[GiaTri] [int] NULL,
 CONSTRAINT [PK_PhanQuyen] PRIMARY KEY CLUSTERED 
(
	[TenDangNhap] ASC,
	[MaMenu] ASC,
	[MaTT] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[PhongBan]    Script Date: 31/07/2020 4:34:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PhongBan](
	[MaPB] [int] IDENTITY(1,1) NOT NULL,
	[TenPB] [nvarchar](50) NULL,
 CONSTRAINT [PK_PhongBan] PRIMARY KEY CLUSTERED 
(
	[MaPB] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[QuyenDangNhap]    Script Date: 31/07/2020 4:34:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[QuyenDangNhap](
	[MaQuyen] [int] IDENTITY(1,1) NOT NULL,
	[TenQuyen] [nchar](10) NULL,
 CONSTRAINT [PK_QuyenDangNhap] PRIMARY KEY CLUSTERED 
(
	[MaQuyen] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TaiKhoan]    Script Date: 31/07/2020 4:34:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TaiKhoan](
	[TKID] [int] IDENTITY(1,1) NOT NULL,
	[MaNV] [nchar](9) NULL,
	[MaPM] [int] NULL,
	[TenTK] [nvarchar](50) NULL,
	[NgayCap] [date] NULL,
	[TrangThai] [int] NULL,
	[GhiChu] [nvarchar](100) NULL,
	[MaTinhTrang] [int] NULL,
 CONSTRAINT [PK_TaiKhoan] PRIMARY KEY CLUSTERED 
(
	[TKID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ThaoTac]    Script Date: 31/07/2020 4:34:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ThaoTac](
	[MaTT] [int] IDENTITY(1,1) NOT NULL,
	[TenTT] [nvarchar](20) NULL,
 CONSTRAINT [PK_ThaoTac] PRIMARY KEY CLUSTERED 
(
	[MaTT] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TinhTrang]    Script Date: 31/07/2020 4:34:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TinhTrang](
	[MaTinhTrang] [int] IDENTITY(1,1) NOT NULL,
	[TenTinhTrang] [nvarchar](20) NULL,
 CONSTRAINT [PK_TinhTrang] PRIMARY KEY CLUSTERED 
(
	[MaTinhTrang] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET IDENTITY_INSERT [dbo].[ChucVu] ON 

INSERT [dbo].[ChucVu] ([MaCV], [TenCV]) VALUES (1, N'Nhân viên')
INSERT [dbo].[ChucVu] ([MaCV], [TenCV]) VALUES (2, N'Kỹ thuật viên')
INSERT [dbo].[ChucVu] ([MaCV], [TenCV]) VALUES (3, N'Phụ trách')
INSERT [dbo].[ChucVu] ([MaCV], [TenCV]) VALUES (4, N'Quản đốc')
SET IDENTITY_INSERT [dbo].[ChucVu] OFF
SET IDENTITY_INSERT [dbo].[GioiTinh] ON 

INSERT [dbo].[GioiTinh] ([MaGT], [TenGT]) VALUES (1, N'Nam       ')
INSERT [dbo].[GioiTinh] ([MaGT], [TenGT]) VALUES (2, N'Nữ        ')
INSERT [dbo].[GioiTinh] ([MaGT], [TenGT]) VALUES (6, N'Không rõ  ')
SET IDENTITY_INSERT [dbo].[GioiTinh] OFF
INSERT [dbo].[NhanVien] ([MaNV], [TenNV], [MaCV], [MaPB], [DiaChi], [SDT], [SoMayLe], [MaGT], [Email], [NgayVaoLam]) VALUES (N'HPDQ00020', N'Nguyen Van Hoai', 1, 3, N'Quang Ngai', N'0961789789 ', 7500, 1, N'nguyenvanhoai@hoaphat.com.vn', CAST(0x64410B00 AS Date))
INSERT [dbo].[NhanVien] ([MaNV], [TenNV], [MaCV], [MaPB], [DiaChi], [SDT], [SoMayLe], [MaGT], [Email], [NgayVaoLam]) VALUES (N'HPDQ00021', N'Nguyen Van Hoai', 1, 3, N'Quang Ngai', N'0961789789 ', 7500, 1, N'nguyenvanhoai@hoaphat.com.vn', CAST(0x64410B00 AS Date))
INSERT [dbo].[NhanVien] ([MaNV], [TenNV], [MaCV], [MaPB], [DiaChi], [SDT], [SoMayLe], [MaGT], [Email], [NgayVaoLam]) VALUES (N'HPDQ00022', N'Nguyen Van Hoai', 1, 3, N'Quang Ngai', N'0961789789 ', 7500, 1, N'nguyenvanhoai@hoaphat.com.vn', CAST(0x64410B00 AS Date))
INSERT [dbo].[NhanVien] ([MaNV], [TenNV], [MaCV], [MaPB], [DiaChi], [SDT], [SoMayLe], [MaGT], [Email], [NgayVaoLam]) VALUES (N'HPDQ00023', N'Nguyen Van Hoai', 1, 3, N'Quang Ngai', N'0961789789 ', 7500, 1, N'nguyenvanhoai@hoaphat.com.vn', CAST(0x64410B00 AS Date))
INSERT [dbo].[NhanVien] ([MaNV], [TenNV], [MaCV], [MaPB], [DiaChi], [SDT], [SoMayLe], [MaGT], [Email], [NgayVaoLam]) VALUES (N'HPDQ00024', N'Nguyen Van Hoai', 1, 3, N'Quang Ngai', N'0961789789 ', 7500, 1, N'nguyenvanhoai@hoaphat.com.vn', CAST(0x64410B00 AS Date))
INSERT [dbo].[NhanVien] ([MaNV], [TenNV], [MaCV], [MaPB], [DiaChi], [SDT], [SoMayLe], [MaGT], [Email], [NgayVaoLam]) VALUES (N'HPDQ00025', N'Nguyen Van Hoai', 1, 3, N'Quang Ngai', N'0961789789 ', 7500, 1, N'nguyenvanhoai@hoaphat.com.vn', CAST(0x64410B00 AS Date))
INSERT [dbo].[NhanVien] ([MaNV], [TenNV], [MaCV], [MaPB], [DiaChi], [SDT], [SoMayLe], [MaGT], [Email], [NgayVaoLam]) VALUES (N'HPDQ00026', N'Nguyen Van Hoai', 1, 3, N'Quang Ngai', N'0961789789 ', 7500, 1, N'nguyenvanhoai@hoaphat.com.vn', CAST(0x64410B00 AS Date))
INSERT [dbo].[NhanVien] ([MaNV], [TenNV], [MaCV], [MaPB], [DiaChi], [SDT], [SoMayLe], [MaGT], [Email], [NgayVaoLam]) VALUES (N'HPDQ00027', N'Nguyen Van Hoai', 1, 3, N'Quang Ngai', N'0961789789 ', 7500, 1, N'nguyenvanhoai@hoaphat.com.vn', CAST(0x64410B00 AS Date))
INSERT [dbo].[NhanVien] ([MaNV], [TenNV], [MaCV], [MaPB], [DiaChi], [SDT], [SoMayLe], [MaGT], [Email], [NgayVaoLam]) VALUES (N'HPDQ00028', N'Nguyen Van Hoai', 1, 3, N'Quang Ngai', N'0961789789 ', 7500, 1, N'nguyenvanhoai@hoaphat.com.vn', CAST(0x64410B00 AS Date))
INSERT [dbo].[NhanVien] ([MaNV], [TenNV], [MaCV], [MaPB], [DiaChi], [SDT], [SoMayLe], [MaGT], [Email], [NgayVaoLam]) VALUES (N'HPDQ00029', N'Nguyen Van Hoai', 1, 3, N'Quang Ngai', N'0961789789 ', 7500, 1, N'nguyenvanhoai@hoaphat.com.vn', CAST(0x64410B00 AS Date))
INSERT [dbo].[NhanVien] ([MaNV], [TenNV], [MaCV], [MaPB], [DiaChi], [SDT], [SoMayLe], [MaGT], [Email], [NgayVaoLam]) VALUES (N'HPDQ00030', N'Nguyen Van Hoai', 1, 3, N'Quang Ngai', N'0961789789 ', 7500, 1, N'nguyenvanhoai@hoaphat.com.vn', CAST(0x64410B00 AS Date))
INSERT [dbo].[NhanVien] ([MaNV], [TenNV], [MaCV], [MaPB], [DiaChi], [SDT], [SoMayLe], [MaGT], [Email], [NgayVaoLam]) VALUES (N'HPDQ00031', N'Nguyen Van Hoai', 1, 3, N'Quang Ngai', N'0961789789 ', 7500, 1, N'nguyenvanhoai@hoaphat.com.vn', CAST(0x64410B00 AS Date))
INSERT [dbo].[NhanVien] ([MaNV], [TenNV], [MaCV], [MaPB], [DiaChi], [SDT], [SoMayLe], [MaGT], [Email], [NgayVaoLam]) VALUES (N'HPDQ00032', N'Nguyen Van Hoai', 1, 3, N'Quang Ngai', N'0961789789 ', 7500, 1, N'nguyenvanhoai@hoaphat.com.vn', CAST(0x64410B00 AS Date))
INSERT [dbo].[NhanVien] ([MaNV], [TenNV], [MaCV], [MaPB], [DiaChi], [SDT], [SoMayLe], [MaGT], [Email], [NgayVaoLam]) VALUES (N'HPDQ00033', N'Nguyen Van Hoai', 1, 3, N'Quang Ngai', N'0961789789 ', 7500, 1, N'nguyenvanhoai@hoaphat.com.vn', CAST(0x64410B00 AS Date))
INSERT [dbo].[NhanVien] ([MaNV], [TenNV], [MaCV], [MaPB], [DiaChi], [SDT], [SoMayLe], [MaGT], [Email], [NgayVaoLam]) VALUES (N'HPDQ00034', N'Nguyen Van Hoai', 1, 3, N'Quang Ngai', N'0961789789 ', 7500, 1, N'nguyenvanhoai@hoaphat.com.vn', CAST(0x64410B00 AS Date))
INSERT [dbo].[NhanVien] ([MaNV], [TenNV], [MaCV], [MaPB], [DiaChi], [SDT], [SoMayLe], [MaGT], [Email], [NgayVaoLam]) VALUES (N'HPDQ00035', N'Nguyen Van Hoai', 1, 3, N'Quang Ngai', N'0961789789 ', 7500, 1, N'nguyenvanhoai@hoaphat.com.vn', CAST(0x64410B00 AS Date))
INSERT [dbo].[NhanVien] ([MaNV], [TenNV], [MaCV], [MaPB], [DiaChi], [SDT], [SoMayLe], [MaGT], [Email], [NgayVaoLam]) VALUES (N'HPDQ00036', N'Nguyen Van Hoai', 1, 3, N'Quang Ngai', N'0961789789 ', 7500, 1, N'nguyenvanhoai@hoaphat.com.vn', CAST(0x64410B00 AS Date))
INSERT [dbo].[NhanVien] ([MaNV], [TenNV], [MaCV], [MaPB], [DiaChi], [SDT], [SoMayLe], [MaGT], [Email], [NgayVaoLam]) VALUES (N'HPDQ00037', N'Nguyen Van Hoai', 1, 3, N'Quang Ngai', N'0961789789 ', 7500, 1, N'nguyenvanhoai@hoaphat.com.vn', CAST(0x64410B00 AS Date))
SET IDENTITY_INSERT [dbo].[PhongBan] ON 

INSERT [dbo].[PhongBan] ([MaPB], [TenPB]) VALUES (1, N'Ban Công nghệ thông tin')
INSERT [dbo].[PhongBan] ([MaPB], [TenPB]) VALUES (2, N'Phòng An toàn môi trường')
INSERT [dbo].[PhongBan] ([MaPB], [TenPB]) VALUES (3, N'Nhà máy Cơ điện')
SET IDENTITY_INSERT [dbo].[PhongBan] OFF
ALTER TABLE [dbo].[DangKy]  WITH CHECK ADD  CONSTRAINT [FK_DangKy_NhanVien] FOREIGN KEY([MaNV])
REFERENCES [dbo].[NhanVien] ([MaNV])
GO
ALTER TABLE [dbo].[DangKy] CHECK CONSTRAINT [FK_DangKy_NhanVien]
GO
ALTER TABLE [dbo].[DangKy]  WITH CHECK ADD  CONSTRAINT [FK_DangKy_QuyenDangNhap] FOREIGN KEY([MaQuyen])
REFERENCES [dbo].[QuyenDangNhap] ([MaQuyen])
GO
ALTER TABLE [dbo].[DangKy] CHECK CONSTRAINT [FK_DangKy_QuyenDangNhap]
GO
ALTER TABLE [dbo].[PhanQuyen]  WITH CHECK ADD  CONSTRAINT [FK_PhanQuyen_DangKy] FOREIGN KEY([TenDangNhap])
REFERENCES [dbo].[DangKy] ([TenDangNhap])
GO
ALTER TABLE [dbo].[PhanQuyen] CHECK CONSTRAINT [FK_PhanQuyen_DangKy]
GO
ALTER TABLE [dbo].[PhanQuyen]  WITH CHECK ADD  CONSTRAINT [FK_PhanQuyen_Menu] FOREIGN KEY([MaMenu])
REFERENCES [dbo].[Menu] ([MaMenu])
GO
ALTER TABLE [dbo].[PhanQuyen] CHECK CONSTRAINT [FK_PhanQuyen_Menu]
GO
ALTER TABLE [dbo].[PhanQuyen]  WITH CHECK ADD  CONSTRAINT [FK_PhanQuyen_ThaoTac] FOREIGN KEY([MaTT])
REFERENCES [dbo].[ThaoTac] ([MaTT])
GO
ALTER TABLE [dbo].[PhanQuyen] CHECK CONSTRAINT [FK_PhanQuyen_ThaoTac]
GO
ALTER TABLE [dbo].[TaiKhoan]  WITH CHECK ADD  CONSTRAINT [FK_TaiKhoan_NhanVien] FOREIGN KEY([MaNV])
REFERENCES [dbo].[NhanVien] ([MaNV])
GO
ALTER TABLE [dbo].[TaiKhoan] CHECK CONSTRAINT [FK_TaiKhoan_NhanVien]
GO
ALTER TABLE [dbo].[TaiKhoan]  WITH CHECK ADD  CONSTRAINT [FK_TaiKhoan_PhanMem] FOREIGN KEY([MaPM])
REFERENCES [dbo].[PhanMem] ([MaPM])
GO
ALTER TABLE [dbo].[TaiKhoan] CHECK CONSTRAINT [FK_TaiKhoan_PhanMem]
GO
ALTER TABLE [dbo].[TaiKhoan]  WITH CHECK ADD  CONSTRAINT [FK_TaiKhoan_TinhTrang] FOREIGN KEY([MaTinhTrang])
REFERENCES [dbo].[TinhTrang] ([MaTinhTrang])
GO
ALTER TABLE [dbo].[TaiKhoan] CHECK CONSTRAINT [FK_TaiKhoan_TinhTrang]
GO
