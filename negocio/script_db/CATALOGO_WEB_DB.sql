use master
go
create database CATALOGO_WEB_DB
go
use CATALOGO_WEB_DB
go
USE [CATALOGO_WEB_DB]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[MARCAS](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [Descripcion] [varchar](50) NULL,
 CONSTRAINT [PK_MARCAS] PRIMARY KEY CLUSTERED 
(
    [Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CATEGORIAS]    Script Date: 08/09/2019 10:32:14 a.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CATEGORIAS](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [Descripcion] [varchar](50) NULL,
 CONSTRAINT [PK_CATEGORIAS] PRIMARY KEY CLUSTERED 
(
    [Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ARTICULOS]    Script Date: 08/09/2019 10:32:24 a.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ARTICULOS](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [Codigo] [varchar](50) NULL,
    [Nombre] [varchar](50) NULL,
    [Descripcion] [varchar](150) NULL,
    [IdMarca] [int] NULL,
    [IdCategoria] [int] NULL,
    [ImagenUrl] [varchar](1000) NULL,
    [Precio] [money] NULL,
 CONSTRAINT [PK_ARTICULOS] PRIMARY KEY CLUSTERED 
(
    [Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
create table USERS(
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [email] [varchar](100) NOT NULL,
    [pass] [varchar](20) NOT NULL,
    [nombre] [varchar](50) NULL,
    [apellido] [varchar](50) NULL,
    [urlImagenPerfil] [varchar](500) NULL,
    [admin] [bit] NOT NULL DEFAULT 0
)
go
create table FAVORITOS(
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [IdUser] [int] NOT NULL,
    [IdArticulo] [int] NOT NULL
)
go
insert into MARCAS values ('Samsung'), ('Apple'), ('Sony'), ('Huawei'), ('Motorola')
insert into CATEGORIAS values ('Celulares'),('Televisores'), ('Media'), ('Audio')
insert into ARTICULOS values ('S01', 'Galaxy S10', 'Una canoa cara', 1, 1, 'https://images.samsung.com/is/image/samsung/assets/ar/p6_gro2/p6_initial_mktpd/smartphones/galaxy-s10/specs/galaxy-s10-plus_specs_design_colors_prism_black.jpg?$163_346_PNG$', 69.999),
('M03', 'Moto G Play 7ma Gen', 'Ya siete de estos?', 5, 1, 'https://www.motorola.cl/arquivos/moto-g7-play-img-product.png?v=636862863804700000', 15699),
('S99', 'Play 4', 'Ya no se cuantas versiones hay', 3, 3, 'sin_imagen_para_que_de_error....muejeje', 35000),
('S56', 'Bravia 55', 'Alta tele', 3, 2, 'https://intercompras.com/product_thumb_keepratio_2.php?img=images/product/SONY_KDL-55W950A.jpg&w=650&h=450', 49500),
('A23', 'Apple TV', 'lindo loro', 2, 3, 'https://store.storeimages.cdn-apple.com/4668/as-images.apple.com/is/rfb-apple-tv-4k?wid=1144&hei=1144&fmt=jpeg&qlt=80&.v=1513897159574', 7850)
insert into USERS (email, pass, admin) values ('admin@admin.com', 'admin', 1)
insert into USERS (email, pass, admin) values ('user@user.com', 'user', 0)
select * from ARTICULOS

select * from USERS


SELECT 
                A.Id, A.Codigo, A.Nombre, A.Descripcion, 
                A.IdMarca, M.Descripcion Marca,
                A.IdCategoria, C.Descripcion Categoria,
                A.ImagenUrl, A.Precio
            FROM ARTICULOS A
            LEFT JOIN MARCAS M ON M.Id = A.IdMarca
            LEFT JOIN CATEGORIAS C ON C.Id = A.IdCategoria