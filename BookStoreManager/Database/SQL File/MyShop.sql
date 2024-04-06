USE MASTER
GO
IF DB_ID('MYSHOP') IS NOT NULL
	DROP DATABASE MYSHOP
GO

--CREATE DATABASE
CREATE DATABASE MYSHOP
GO
USE MYSHOP
GO

CREATE TABLE ACCOUNT
(
	ACCOUNT_ID INT NOT NULL IDENTITY,
	USERNAME VARCHAR(20) NOT NULL UNIQUE,
	PASS NVARCHAR(MAX) NOT NULL,
	ENTROPY NVARCHAR(MAX) NOT NULL,
	FULLNAME NVARCHAR(50)

	CONSTRAINT PK_ACCOUNT
	PRIMARY KEY (ACCOUNT_ID)
)

CREATE TABLE CATEGORY
(
	CATEGORY_ID INT NOT NULL IDENTITY,
	CATEGORY_NAME NVARCHAR(50) NOT NULL UNIQUE,

	CONSTRAINT PK_CATEGORY
	PRIMARY KEY (CATEGORY_ID)
)

CREATE TABLE BOOK
(
	BOOK_ID INT NOT NULL IDENTITY,
	BOOK_NAME NVARCHAR(50) NOT NULL,
	PRICE INT NOT NULL,
	AUTHOR NVARCHAR(50) NOT NULL,
	IMG NVARCHAR(100),

	CONSTRAINT PK_BOOK
	PRIMARY KEY (BOOK_ID)
)

CREATE TABLE BOOK_CATEGORY
(
	BOOK_ID INT NOT NULL,
	CATEGORY_ID INT NOT NULL,

	CONSTRAINT FK_BOOK_CATEGORY_BOOK 
	FOREIGN KEY (BOOK_ID) 
	REFERENCES BOOK(BOOK_ID),

	CONSTRAINT FK_BOOK_CATEGORY_CATEGORY 
	FOREIGN KEY (CATEGORY_ID) 
	REFERENCES CATEGORY(CATEGORY_ID),

	CONSTRAINT PK_BOOK_CATEGORY 
	PRIMARY KEY (BOOK_ID, CATEGORY_ID)
)

CREATE TABLE ORDER_LIST
(
	ORDER_ID INT NOT NULL IDENTITY,
	CUSTOMER_NAME NVARCHAR(50) NOT NULL,
	ORDER_DATE DATE NOT NULL,
	PRICE INT DEFAULT(0),

	CONSTRAINT PK_ORDER_LIST
	PRIMARY KEY (ORDER_ID)
)

CREATE TABLE ORDER_ITEM
(
	ORDER_ID INT NOT NULL,
	BOOK_ID INT NOT NULL,
	QUANTITY INT NOT NULL DEFAULT(0),

	CONSTRAINT FK_ORDER_ITEM_ORDER_LIST
	FOREIGN KEY (ORDER_ID)
	REFERENCES ORDER_LIST(ORDER_ID),

	CONSTRAINT FK_ORDER_ITEM_BOOK
	FOREIGN KEY (BOOK_ID)
	REFERENCES BOOK(BOOK_ID),
)

INSERT ACCOUNT(USERNAME, PASS, ENTROPY, FULLNAME)
VALUES	( 'admin', 'AQAAANCMnd8BFdERjHoAwE/Cl+sBAAAAERQX5xOj8EWQKUBLmg+3JgAAAAACAAAAAAAQZgAAAAEAACAAAAAIYJ1RdaqoENndp+RZ7OCxf9afuyTmev+ukmvtVbbm/gAAAAAOgAAAAAIAACAAAAAo5Ee4hx8Hv4xbzyqnJ676165sN8+VtmduEXa7PCURkhAAAADCWUhQt6tjsZ2Wogn50vV1QAAAAIvgSs/NROoPcLKtOoWmaoouWvAfHEcw8/6G3686oUXBOJPG0b4RqW3NaIoCT6P2S0cUlh3cq3PTyX7AMaMa5Qo=', 'iBwdigFSr3tVbqx0hg7NYRLFOa8XhQNHCZ8zfBzpLvo=', 'admin')

INSERT CATEGORY(CATEGORY_NAME)
VALUES  ('Novel'),
		('Action'),
		('Adult'),
		('Adventure'),
		('Business'),
		('Childrens mystery'),
		('Classic'),
		('Classic short story'),
		('Comedy'),
		('Drama'),
		('Family saga'),
		('Fantasy'),
		('Historical'),
		('Horror'),
		('Isekai'),
		('Josei'),
		('LGBTQ+ fiction'),
		('Martial arts novel'),
		('Middle grade fiction'),
		('Mystery'),
		('Non-fiction'),
		('Psychological'),
		('Romance'),
		('School Life'),
		('Scientific'),
		('Sci-Fi'),
		('Seinen'),
		('Short story'),
		('Shoujo'),
		('Shounen'),
		('Single father novel'),
		('Single mother novel'),
		('Slice of Life'),
		('Speculative'),
		('Sports'),
		('Supernatural'),
		('War'),
		('Young adult fiction'),
		('Tragedy'),
		('Thriller'), 
		('Quest'),
		('Self-help')

INSERT BOOK(BOOK_NAME, PRICE, AUTHOR)
VALUES  (N'Đắc nhân tâm', 86000, N'Dale Carnegie'),
		(N'One Piece', 21250, N'Eiichiro Oda'),
		(N'Attack On Titan', 144400, N'Isayama Hajime'),
		(N'Naruto', 21500, N'Masashi Kishimoto'),
		(N'Cardcaptor Sakura', 25000, N'CLAMP'),
		(N'Cardcaptor Sakura: Clear Card-hen', 175750, N'CLAMP'),
		(N'Trốn lên mái nhà để khóc', 72200, N'Lam'),
		(N'Frieren - Pháp sư tiễn táng', 42750, N'Yamada Kanehito'),
		(N'Thám tử lừng danh Conan', 23750, N'Gosho Aoyama'),
		(N'Dược sư tự sự', 44650, N'Natsu Hyuuga'),
		(N'Nhà giả kim', 61620, N'Paulo Coelho')

INSERT BOOK_CATEGORY(BOOK_ID, CATEGORY_ID)
VALUES  (1, 22),
		(2, 2),
		(2, 4),
		(2, 12),
		(2, 9),
		(3, 2),
		(3, 13),
		(3, 14),
		(3, 20),
		(3, 30),
		(3, 39),
		(4, 2),
		(4, 4),
		(4, 9),
		(4, 10),
		(4, 12),
		(4, 30),
		(4, 36),
		(5, 4),
		(5, 9),
		(5, 12),
		(5, 23),
		(5, 29),
		(6, 4),
		(6, 9),
		(6, 12),
		(6, 23),
		(6, 29),
		(7, 28),
		(8, 4),
		(8, 10),
		(8, 12),
		(9, 20),
		(9, 40),
		(10, 10),
		(10, 20),
		(10, 23),
		(11, 41),
		(11, 4),
		(11, 12)

INSERT ORDER_LIST(CUSTOMER_NAME, ORDER_DATE, PRICE)
VALUES  (N'Nguyễn Văn Anh', '03/05/2024', 476500),
		(N'Trần Chí Hào', '03/10/2024', 235750),
		(N'Trần Thái Sơn', '03/03/2024', 941500),
		(N'Vương Huỳnh Phúc Tịnh', '03/09/2024', 966400),
		(N'Ngô Toàn Trung', '03/10/2024', 885150),
		(N'Đào Nguyễn Nhật Minh', '04/02/2024', 685100),
		(N'Nguyễn Phùng Tài', '10/20/2023', 738250),
		(N'Đào Nhật Minh', '04/01/2024', 685100)

INSERT ORDER_ITEM(ORDER_ID, BOOK_ID, QUANTITY)
VALUES  (1, 5, 5),
		(1, 6, 2),
		(2, 2, 3),
		(2, 1, 1),
		(2, 4, 4),
		(3, 5, 2),
		(3, 6, 3),
		(3, 1, 3),
		(3, 2, 5),
		(4, 5, 4),
		(4, 3, 6),
		(5, 5, 1),
		(5, 1, 3),
		(5, 6, 2),
		(5, 3, 1),
		(5, 2, 5),
		(6, 3, 4),
		(6, 4, 5),
		(7, 1, 1),
		(7, 5, 5),
		(7, 6, 3),
		(8, 3, 4),
		(8, 4, 5)

SELECT BOOK.BOOK_ID, BOOK.BOOK_NAME,SUM(ORDER_ITEM.QUANTITY) AS QUANTITY , SUM(ORDER_ITEM.QUANTITY)*BOOK.PRICE AS REVENUE
FROM BOOK JOIN ORDER_ITEM ON BOOK.BOOK_ID = ORDER_ITEM.BOOK_ID
GROUP BY BOOK.BOOK_ID, BOOK.BOOK_NAME, BOOK.PRICE