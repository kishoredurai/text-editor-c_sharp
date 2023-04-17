create database texteditor;


create table content 
( FileId int identity primary key,
	FileName varchar(30) unique,
	FileContent varchar(300)
)

select * from content;

alter table content
add FileOwner varchar(30);

insert into content
values
('test.txt','sdfsdfsdvvgsdfgsfgsdfgdgdsgsdfg','kumar'),
('test1.txt','sdfsdfsdvvgsdfgsfgsdfgdgdsgsdfg','kumar');
