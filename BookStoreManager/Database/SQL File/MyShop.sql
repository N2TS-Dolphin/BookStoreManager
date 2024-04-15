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

CREATE TABLE CUSTOMER
(
	CUSTOMER_ID INT NOT NULL IDENTITY,
	CUSTOMER_NAME NVARCHAR(50) NOT NULL,
	CUSTOMER_EMAIL VARCHAR(50),
	CUSTOMER_PHONE VARCHAR(50),

	CONSTRAINT PK_CUSTOMER
	PRIMARY KEY (CUSTOMER_ID)
)

CREATE TABLE ORDER_LIST
(
    ORDER_ID INT NOT NULL IDENTITY,
    CUSTOMER_ID INT NOT NULL,
    ORDER_DATE DATE NOT NULL,
    PRICE INT DEFAULT(0),
    ORDER_ADDRESS NVARCHAR(200),

    CONSTRAINT FK_CUSTOMER_ORDER_LIST 
    FOREIGN KEY (CUSTOMER_ID) 
    REFERENCES CUSTOMER(CUSTOMER_ID),

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
VALUES	( 'admin', 'AQAAANCMnd8BFdERjHoAwE/Cl+sBAAAAERQX5xOj8EWQKUBLmg+3JgAAAAACAAAAAAAQZgAAAAEAACAAAAAIYJ1RdaqoENndp+RZ7OCxf9afuyTmev+ukmvtVbbm/gAAAAAOgAAAAAIAACAAAAAo5Ee4hx8Hv4xbzyqnJ676165sN8+VtmduEXa7PCURkhAAAADCWUhQt6tjsZ2Wogn50vV1QAAAAIvgSs/NROoPcLKtOoWmaoouWvAfHEcw8/6G3686oUXBOJPG0b4RqW3NaIoCT6P2S0cUlh3cq3PTyX7AMaMa5Qo=', 'iBwdigFSr3tVbqx0hg7NYRLFOa8XhQNHCZ8zfBzpLvo=', 'admin' )

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

INSERT CUSTOMER(CUSTOMER_NAME, CUSTOMER_EMAIL, CUSTOMER_PHONE)
VALUES  (N'Nguyễn Văn Anh', 'nvanh@gmail.com', '0938092736'),					--1--
		(N'Trần Chí Hào', 'tccao@gmail.com', '0902846294'),						--2--
		(N'Trần Thái Sơn', 'ttson@gmail.com', '0932735284'),					--3--
		(N'Vương Huỳnh Phúc Tịnh', 'vhpthinh@gmail.com', '0902264926'),			--4--
		(N'Ngô Toàn Trung', 'nttrung@gmail.com', '0932725494'),					--5--
		(N'Đào Nguyễn Nhật Minh', 'dnnminh@gmail.com', '0902583511'),			--6--
		(N'Nguyễn Phùng Tài', 'nptai@gmail.com', '093896243'),					--7--
		(N'Đào Nhật Minh', 'dnminh@gmail.com', '0902936142'),					--8--
		(N'Huỳnh Phúc Tịnh', 'hptinh@gmail.com', '0938086352'),					--9--
		(N'Lê Quốc An', 'lqan@gmail.com', '0932872445'),						--10--
		(N'Trần Ngọc Mến', 'tnmen@gmail.com', '0902284936'),					--11--
		(N'Nguyễn Thank Kiều', 'ntkieu@gmail.com', '0938936243'),				--12--
		(N'Phương Quỳnh', 'pquynh@gmail.com', '0932583846'),					--13--
		(N'Chu Thị Hồng Thương', 'cththuong@gmail.com', '0932999625'),			--14--
		(N'Nguyễn Thanh', 'nthanh@gmail.com', '0902134876'),					--15--
		(N'Duy Minh', 'dminh@gmail.com', '0938983567'),							--16--
		(N'Nguyễn Thanh Cảnh', 'ntcanh@gmail.com', '0902947254'),				--17--
		(N'Soon Jin Woo', 'sjwoo@gmail.com', '0902873647'),						--18--
		(N'Mẫn Kì', 'mki@gmail.com', '0938235987'),								--19--
		(N'Thế Hưng', 'thung@gmail.com', '0902944722'),							--20--
		(N'Nam Tuấn', 'ntuan@gmail.com', '0932855262'),							--21--
		(N'Trung Phúc', 'tphuc@gmail.com', '0938876893'),						--22--
		(N'Kim Trân', 'ktran@gmail.com', '0902764153'),								--23--
		(N'Phương Phương', 'pphuong@gmail.com', '0938852573'),					--24--
		(N'Quỳnh Quỳnh', 'qquynh@gmail.com', '0938427496'),						--25--
		(N'David', 'david@gmail.com', '0932853888'),							--26--
		(N'Jeon Jungkook', 'jjkook@gmail.com', '0938764245'),					--27--
		(N'Nguyễn Thúc Thuỳ Tiên', 'ntttien@gmail.com', '090222777'),			--28--
		(N'Đông Mẫn', 'dman@gmail.com', '0932999999')							--29--


INSERT INTO ORDER_LIST (CUSTOMER_ID, ORDER_DATE, PRICE, ORDER_ADDRESS)
VALUES  
    (1, '03/05/2024', 476500, N'456 Đường Lê Lợi, Quận 3, TP.HCM'),	
    (2, '03/10/2024', 235750, N'123 Đường Nguyễn Văn Linh, Quận 1, TP.HCM'),	
    (3, '03/03/2024', 941500, N'789 Đường Hùng Vương, Quận 5, TP.HCM'),		
    (4, '03/09/2024', 966400, N'27 Đường Lý Thường Kiệt, Quận 10, TP.HCM'),	
    (5, '03/10/2024', 885150, N'64 Đường Bến Vân Đồn, Quận 4, TP.HCM'),		
    (6, '04/02/2024', 685100, N'123 Đường Nguyễn Thị Minh Khai, Quận 1, TP.HCM'),	
    (7, '10/20/2023', 738250, N'456 Đường Trần Hưng Đạo, Quận 5, TP.HCM'),	
    (8, '04/01/2024', 685100, N'789 Đường Phan Chu Trinh, Quận 1, TP.HCM'),	
    (9, '04/05/2024', 399000, N'123 Đường Nguyễn Thái Học, Quận 5, TP.HCM'),	
    (10, '04/03/2024', 453970, N'456 Đường Trần Phú, Quận 10, TP.HCM'),		
    (11, '04/02/2024', 401550, N'27 Đường Lê Duẩn, Quận 1, TP.HCM'),		
    (12, '03/24/2024', 555520, N'789 Đường Hai Bà Trưng, Quận 3, TP.HCM'),	
    (13, '03/09/2024', 158200, N'64 Đường Lê Văn Sỹ, Quận 3, TP.HCM'),		
    (14, '04/14/2023', 4495050, N'27 Đường Đinh Tiên Hoàng, Quận 1, TP.HCM'),	
    (15, '03/01/2024', 1359250, N'456 Đường Nguyễn Thị Diệu, Quận 10, TP.HCM'),	
    (16, '02/18/2024', 281120, N'123 Đường Lê Lai, Quận 1, TP.HCM'),		
    (17, '02/19/2024', 623250, N'789 Đường Nguyễn Huệ, Quận 1, TP.HCM'),		
    (18, '03/24/2024', 1442200, N'64 Đường Hai Triều, Quận 5, TP.HCM'),	
    (19, '03/26/2024', 412120, N'27 Đường Trần Quang Khải, Quận 1, TP.HCM'),	
    (20, '10/13/2023', 519600, N'456 Đường Lê Lai, Quận 4, TP.HCM'),		
    (21, '09/12/2023', 289000, N'123 Đường Lê Văn Thịnh, Quận 7, TP.HCM'),	
    (22, '05/03/2023', 214750, N'789 Đường Nguyễn Hữu Thọ, Quận 4, TP.HCM'),	
    (23, '12/03/2023', 229770, N'64 Đường Lê Quang Định, Quận Bình Thạnh, TP.HCM'),	
    (24, '05/10/2023', 386740, N'27 Đường Lê Hồng Phong, Quận 10, TP.HCM'),	
    (25, '09/02/2023', 386500, N'456 Đường Cách Mạng Tháng 8, Quận 3, TP.HCM'),	
    (26, '11/11/2024', 1029700, N'123 Đường Trần Quốc Toản, Quận Bình Thạnh, TP.HCM'),	
    (27, '09/12/2023', 1391250, N'789 Đường Nguyễn Văn Cừ, Quận 1, TP.HCM'),	
    (28, '03/23/2024', 801760, N'64 Đường Nam Kỳ Khởi Nghĩa, Quận 1, TP.HCM'),	
    (29, '06/13/2023', 607870, N'27 Đường Phạm Ngọc Thạch, Quận 3, TP.HCM');



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

SELECT BOOK.BOOK_ID AS ID, BOOK.BOOK_NAME AS NAME,SUM(ORDER_ITEM.QUANTITY) AS QUANTITY , SUM(ORDER_ITEM.QUANTITY)*BOOK.PRICE AS REVENUE FROM BOOK JOIN ORDER_ITEM ON BOOK.BOOK_ID = ORDER_ITEM.BOOK_ID JOIN ORDER_LIST ON ORDER_LIST.ORDER_ID = ORDER_ITEM.ORDER_ID WHERE YEAR(ORDER_LIST.ORDER_DATE) = YEAR(GETDATE()) AND MONTH(ORDER_LIST.ORDER_DATE) = MONTH(GETDATE()) AND DAY(ORDER_LIST.ORDER_DATE) = DAY(GETDATE()) GROUP BY BOOK.BOOK_ID, BOOK.BOOK_NAME, BOOK.PRICE ORDER BY REVENUE DESC