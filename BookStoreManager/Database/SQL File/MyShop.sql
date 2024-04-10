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
VALUES  ('Novel'),					--1--
		('Action'),					--2--
		('Adult'),					--3--
		('Adventure'),				--4--
		('Business'),				--5--
		('Childrens mystery'),		--6--
		('Classic'),				--7--
		('Classic short story'),	--8--
		('Comedy'),					--9--
		('Drama'),					--10--
		('Family saga'),			--11--
		('Fantasy'),				--12--
		('Historical'),				--13--
		('Horror'),					--14--
		('Isekai'),					--15--
		('Josei'),					--16--
		('LGBTQ+ fiction'),			--17--
		('Martial arts novel'),		--18--
		('Middle grade fiction'),	--19--
		('Mystery'),				--20--
		('Non-fiction'),			--21--
		('Psychological'),			--22--
		('Romance'),				--23--
		('School Life'),			--24--
		('Scientific'),				--25--
		('Sci-Fi'),					--26--
		('Seinen'),					--27--
		('Short story'),			--28--
		('Shoujo'),					--29--
		('Shounen'),				--30--
		('Single father novel'),	--31--
		('Single mother novel'),	--32--
		('Slice of Life'),			--33--
		('Speculative'),			--34--
		('Sports'),					--35--
		('Supernatural'),			--36--
		('War'),					--37--
		('Young adult fiction'),	--38--
		('Tragedy'),				--39--
		('Thriller'), 				--40--
		('Quest'),					--41--
		('Self-help'),				--42--
		('Life Skills'),			--43--
		('Dark Fantasy'),			--44--
		('Econimics')				--45--

INSERT BOOK(BOOK_NAME, PRICE, AUTHOR, IMG)
VALUES  (N'Đắc nhân tâm', 86000, N'Dale Carnegie', N'DacNhanTam.jpg'),									--1--
		(N'One Piece', 21250, N'Eiichiro Oda', N'OnePiece.jpg'),										--2--
		(N'Attack On Titan', 144400, N'Isayama Hajime', N'AttackOnTitan.jpg'),							--3--
		(N'Naruto', 21500, N'Masashi Kishimoto', N'Naruto.jpg'),										--4--
		(N'Cardcaptor Sakura', 25000, N'CLAMP', N'CardcaptorSakura.jpg'),								--5--
		(N'Cardcaptor Sakura: Clear Card-hen', 175750, N'CLAMP', N'CardcaptorSakuraClearCard-hen.jpg'),	--6--
		(N'Trốn lên mái nhà để khóc', 72200, N'Lam', N'TronLenMaiNhaDeKhoc.jpg'),						--7--
		(N'Frieren - Pháp sư tiễn táng', 42750, N'Yamada Kanehito', N'Frieren.jpg'),					--8--
		(N'Thám tử lừng danh Conan', 23750, N'Gosho Aoyama', N'Conan.jpg'),								--9--
		(N'Dược sư tự sự', 44650, N'Natsu Hyuuga', N'KusuriyaNoHitorigoto.jpg'),						--10--
		(N'Nhà giả kim', 61620, N'Paulo Coelho', N'NhaGiaKim.jpg'),										--11--
		(N'7 thói quen để thành đạt', 96000, N'Stephen R Covey', N''),									--12--
		(N'Jujutsu Kaisen', 58500, N'Akutami Gege', N''),												--13--
		(N'13 nguyên tắc nghĩ giàu làm giàu', 100080, N'Napoleon Hill', N''),							--14--
		(N'Bí quyết gây dựng cơ nghiệp bạc tỷ', 116000, N'Adam Khoo', N''),								--15--
		(N'Những tấm lòng cao cả', 135000, N'Edmondo De Amicis', N''),									--16--
		(N'Những cuộc phiêu lưu của Pinocchio', 162000, N'Carlo Collodi', N''),							--17--
		(N'Đồi thỏ', 145000, N'Richard Adams', N''),													--18--
		(N'Combo Harry Potter (7 cuốn)', 1540000, N'J.K.Rowling', N''),									--19--
		(N'Trên đường băng', 80000, N'Tony Buổi Sáng', N'')												--20--

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
		(11, 12),
		(12, 22),
		(12, 42),
		(12, 43),
		(13, 4),
		(13, 36),
		(13, 44),
		(14, 21),
		(14, 42),
		(15, 5),
		(15, 22),
		(15, 43),
		(15, 45),
		(16, 1),
		(17, 4),
		(17, 12),
		(18, 1),
		(18, 4),
		(19, 1),
		(19, 12),
		(20, 22),
		(20, 43)

INSERT ORDER_LIST(CUSTOMER_NAME, ORDER_DATE, PRICE)
VALUES  (N'Nguyễn Văn Anh', '03/05/2024', 476500),					--1--
		(N'Trần Chí Hào', '03/10/2024', 235750),					--2--
		(N'Trần Thái Sơn', '03/03/2024', 941500),					--3--
		(N'Vương Huỳnh Phúc Tịnh', '03/09/2024', 966400),			--4--
		(N'Ngô Toàn Trung', '03/10/2024', 885150),					--5--
		(N'Đào Nguyễn Nhật Minh', '04/02/2024', 685100),			--6--
		(N'Nguyễn Phùng Tài', '10/20/2023', 738250),				--7--
		(N'Đào Nhật Minh', '04/01/2024', 685100),					--8--
		(N'Huỳnh Phúc Tịnh', '04/05/2024', 399000),					--9--
		(N'Lê Quốc An', '04/03/2024', 453970),						--10--
		(N'Trần Ngọc Mến', '04/02/2024', 401550),					--11--
		(N'Nguyễn Thank Kiều', '03/24/2024', 555520),				--12--
		(N'Phương Quỳnh', '03/09/2024', 158200),					--13--
		(N'Chu Thị Hồng Thương', '04/14/2023', 4495050),			--14--
		(N'Nguyễn Thanh', '03/01/2024', 1359250),					--15--
		(N'Duy Minh', '02/18/2024', 281120),						--16--
		(N'Nguyễn Thanh Cảnh', '02/19/2024', 623250),				--17--
		(N'Soon Jin Woo', '03/24/2024', 1442200),					--18--
		(N'Mẫn Kì', '03/26/2024', 412120),							--19--
		(N'Thế Hưng', '10/13/2023', 519600),						--20--
		(N'Nam Tuấn', '09/12/2023', 289000),						--21--
		(N'Trung Phúc', '05/03/2023', 214750),						--22--
		(N'Kim Trân', '12/03/2023', 229770),						--23--
		(N'Phương Phương', '05/10/2023', 386740),					--24--
		(N'Quỳnh Quỳnh', '09/02/2023', 386500),						--25--
		(N'David', '11/11/2024', 1029700),							--26--
		(N'Jeon Jungkook', '09/12/2023', 1391250),					--27--
		(N'Nguyễn Thúc Thuỳ Tiên', '03/23/2024', 801760),			--28--
		(N'Đông Mẫn', '06/13/2023', 607870)							--29--

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
		(8, 4, 5),
		(9, 9, 10),
		(9, 10, 2),
		(9, 7, 1),
		(10, 8, 5),
		(10, 10, 4),
		(10, 11, 1),
		(11, 7, 3),
		(11, 11, 3),
		(12, 2, 9),
		(12, 3, 1),
		(12, 4, 8),
		(12, 10, 1),
		(12, 11, 1),
		(13, 1, 1),
		(13, 7, 1),
		(14, 1, 2),
		(14, 2, 1),
		(14, 3, 7),
		(14, 4, 4),
		(14, 7, 5),
		(14, 8, 8),
		(14, 9, 100),
		(14, 10, 12),
		(15, 1, 1),
		(15, 2, 8),
		(15, 3, 12),
		(15, 9, 3),
		(16, 1, 2),
		(16, 9, 2),
		(16, 11, 1),
		(17, 2, 1),
		(17, 3, 7),
		(18, 1, 1),
		(18, 2, 24),
		(18, 3, 9),
		(18, 7, 1),
		(19, 1, 2),
		(19, 2, 1),
		(19, 3, 1),
		(19, 9, 3),
		(19, 11, 1),
		(20, 1, 3),
		(20, 2, 1),
		(20, 7, 3),
		(20, 9, 1),
		(21, 4, 3),
		(21, 5, 1),
		(21, 6, 1),
		(21, 9, 1),
		(22, 1, 1),	
		(22, 2, 1),	
		(22, 3, 1),	
		(22, 4, 1),
		(23, 7, 2),
		(23, 9, 1),
		(23, 11, 1),
		(24, 1, 1),
		(24, 2, 1),
		(24, 3, 1),
		(24, 4, 1),
		(24, 5, 1),
		(24, 9, 1),
		(24, 11, 2),
		(25, 1, 1),	
		(25, 2, 2),	
		(25, 3, 3),
		(26, 1, 1),	
		(26, 2, 2),
		(26, 5, 1),
		(26, 7, 1),	
		(26, 8, 5),	
		(26, 9, 10),
		(26, 10, 1),
		(26, 11, 5),
		(27, 1, 3),	
		(27, 2, 4),	
		(27, 3, 5),	
		(27, 4, 6),
		(27, 8, 7),	
		(27, 9, 8),	
		(28, 1, 1),	
		(28, 2, 2),	
		(28, 3, 4),	
		(28, 7, 2),	
		(28, 11, 3),
		(29, 8, 10),
		(29, 9, 5),	
		(29, 11, 1)

SELECT BOOK.BOOK_ID, BOOK.BOOK_NAME,SUM(ORDER_ITEM.QUANTITY) AS QUANTITY , SUM(ORDER_ITEM.QUANTITY)*BOOK.PRICE AS REVENUE
FROM BOOK JOIN ORDER_ITEM ON BOOK.BOOK_ID = ORDER_ITEM.BOOK_ID
GROUP BY BOOK.BOOK_ID, BOOK.BOOK_NAME, BOOK.PRICE
ORDER BY QUANTITY