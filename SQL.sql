USE master ;
GO
Drop DATABASE Comments
CREATE DATABASE Comments
--Drop table Comment
USE Comments ;
GO
CREATE TABLE IpAddres
(
Id_ip int IDENTITY(1, 1) PRIMARY KEY not null,
ClientIp nvarchar(100),
)
CREATE TABLE [User]
(
Id_User int IDENTITY(1, 1) PRIMARY KEY not null,
UserName nvarchar(200) not null,
Email nvarchar(200) not null,
HomePage nvarchar(500),
Id_ip int,
CONSTRAINT [FK_Ip] FOREIGN KEY (Id_ip) REFERENCES IpAddres(Id_ip)
)

CREATE TABLE Comment
(
Id_Comment int IDENTITY(1, 1) PRIMARY KEY not null,
DateComment date not null,
Title nvarchar(200) ,
[Message] nvarchar(1500) ,
Parent int,
LVL int,
Id_User int,
CONSTRAINT [FK_User] FOREIGN KEY (Id_User) REFERENCES [User](Id_User),
CONSTRAINT [FK_Parent] FOREIGN KEY (Parent) REFERENCES Comment(Id_Comment)
)

insert into IpAddres values('IpAddres');
insert into [User] values('Awesome NickName','Email@@@','HomePage///',(SELECT MAX(Id_ip) FROM IpAddres));
insert into Comment values(SYSDATETIME (),'Title','[Message]',null,0,(SELECT MAX(Id_User) FROM [User]));
insert into Comment values('12/12/2011 22:22:22','Title222','[Message2342]',null,0,(SELECT MAX(Id_User) FROM [User]));
insert into Comment values('23/01/2022 11:11:11','Title222','[Message2342]',null,0,(SELECT MAX(Id_User) FROM [User]));
insert into [User] values('UFO','Email@@@gggdfs','htt//3/3123',(SELECT MAX(Id_ip) FROM IpAddres));
insert into Comment values('25/05/2017 11:11:11','asdasd','[Message2342]',null,0,(SELECT MAX(Id_User) FROM [User]));
insert into Comment values('01/08/2015 13:13:13','zxczxc','[ujmujmu]',(SELECT MAX(Id_Comment) FROM Comment),1,(SELECT MAX(Id_User) FROM [User]));
insert into Comment values('20/01/2001 13:15:15','Ancient','Lorem ipsum dolor sit amet, consectetur adipisicing elit. Culpa excepturi, expedita eum molestiae error explicabo libero incidunt ab facere ipsam possimus quos repellat minima non neque maio'
,1,1,1);
insert into Comment values('22/11/2013 11:22:54','Ancient was back','Lorem ipsum dolor sit amet, consectetur adipisicing elit. Culpa excepturi, expedita eum molestiae error explicabo libero incidunt ab facere ipsam possimus quos repellat minima non neque maiores temporibus. Nostrum repudiandae maiores corrupti ipsum quisquam doloribus libero temporibus dicta quidem veniam expedita cumque natus ea officia modi, placeat obcaecati facilis, perferendis nulla voluptatum iure quos? Accusantium, officia ex explicabo, voluptate possimus suscipit. Harum maxime, eum ex ipsam veritatis dolores temporibus praesentium magnam numquam sed nam architecto fugit ut vel nulla a autem, velit, perferendis cum saepe accusamus quisquam blanditiis pariatur aspernatur. Expedita ad voluptatibus accusamus. Dolore magni iusto ratione quas possimus.'
,1,2,1);
insert into Comment values('10/01/2011 22:22:22','Error','t ab facere ipsam possimus quos repellat minima non neque maio'
,2,1,2);
insert into Comment values('25/05/2017 11:11:11','asdasd','[Message2342]',1,1,1);
insert into Comment values('11/11/2227 11:11:11','fvfvffff','[bn,mbnm,b]',9,2,2);

select Id_Comment,DateComment,Title,[Message],Parent,LVL,UserName,Email,HomePage from Comment
join  [User]  on  [User].Id_User = Comment.Id_User

