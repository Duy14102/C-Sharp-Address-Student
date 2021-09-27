drop database if exists ProjectCSDL;

create database ProjectCSDL;

use ProjectCSDL;

/* Staffs */
/* Tables */
/* Categories */
/* Items */
/* Invoice */
/* InvoiceDetail */

create table Staffs(
	StaffID int auto_increment primary key,
    Username varchar(100) not null unique,
    Userpass varchar(1000) not null,
    StaffName varchar(100) not null default 'Staff',
    role int not null default 1
);
 
 create table TableFood(
	Tables_ID int auto_increment primary key,
	Tables_Name varchar(100) not null default 'Unnamed Table',     /* Ban 1, Ban duoi, Ban tren */
    Tables_Status int not null default 1    /* Ban trong, Ban co nguoi */
 );
 
 create table Category(
	Category_ID int auto_increment primary key,
    Category_Name varchar(100) not null default 'Unnamed Category'
 );
 
 create table Items(
	Items_ID int auto_increment primary key,
    Items_Name varchar(100) not null default 'Unnamed Item',
    CategoryID_FK int not null,
    Items_Price decimal(20,3) not null default 0,
    foreign key (CategoryID_FK) references Category(Category_ID)
 );
 
 create table Invoices(
	Invoices_ID int auto_increment primary key,
    Invoices_Date datetime default now() not null,
    TableID_FK int not null,
    Invoices_Status int not null default 1,       /* 1: Thanh toan roi , 0: Chua thanh toan */
    foreign key (TableID_FK) references TableFood(Tables_ID)
 );
 
 create table Invoice_details(
    InvoicesID_FK int not null ,
    ItemsID_FK int not null ,
    Items_Price decimal(20,3) not null,
    count int not null default 1,
    constraint pk_OrderDetails primary key(InvoicesID_FK, ItemsID_FK),
    foreign key (InvoicesID_FK) references invoices(Invoices_ID),
    foreign key (ItemsID_FK) references Items(Items_ID) 
 );
 select * from Invoice_details where InvoicesID_FK = 5;
 select c.Items_Name, c.Items_Price, i.count from 
Invoice_details i inner join Items c on i.ItemsID_FK = c.Items_ID where i.InvoicesID_FK =5;

 create user if not exists 'staffpf13'@'localhost' identified by 'staff1234';
 
 grant all on ProjectCSDL.* to 'staffpf13'@'localhost';
 

 insert into Staffs(Username, Userpass, StaffName) values ('staffpf13', '04d4b37015f6ba05077ae49776a76b95', 'staff');
 
 insert into TableFood(Tables_Name) values ('Table 1');
 insert into TableFood(Tables_Name) values ('Table 2');
 insert into TableFood(Tables_Name) values ('Table 3');
 insert into TableFood(Tables_Name) values ('Table 4');
 insert into TableFood(Tables_Name) values ('Table 5');
 insert into TableFood(Tables_Name) values ('Table 6');
 insert into TableFood(Tables_Name) values ('Table 7');
 insert into TableFood(Tables_Name) values ('Table 8');
 insert into TableFood(Tables_Name) values ('Table 9');
 insert into TableFood(Tables_Name) values ('Table 10');
 
 insert into Category(Category_Name) values ('Meat');
 insert into Category(Category_Name) values ('Fish');
 insert into Category(Category_Name) values ('Side Dishes');
 insert into Category(Category_Name) values ('Tofu');
 insert into Category(Category_Name) values ('Drink');
 
 insert into Items(Items_Name, Items_Price, CategoryID_FK) values ('Rice', '5.0', '3');
 insert into Items(Items_Name, Items_Price, CategoryID_FK) values ('Fried Tofu', '10.0', '4');
 insert into Items(Items_Name, Items_Price, CategoryID_FK) values ('Tofu In Tomato Sauce', '10.0', '4');
 insert into Items(Items_Name, Items_Price, CategoryID_FK) values ('Fried Fish', '15.0', '2');
 insert into Items(Items_Name, Items_Price, CategoryID_FK) values ('Stew Fish', '15.0', '2');
 insert into Items(Items_Name, Items_Price, CategoryID_FK) values ('Stew Meat', '15.0', '1');
 insert into Items(Items_Name, Items_Price, CategoryID_FK) values ('Sauteed Spinach', '5.0', '3');
 insert into Items(Items_Name, Items_Price, CategoryID_FK) values ('Fried Meat', '15.0', '1');
 insert into Items(Items_Name, Items_Price, CategoryID_FK) values ('Roast', '20.0', '1');
 insert into Items(Items_Name, Items_Price, CategoryID_FK) values ('Sweet And Sour Ribs', '20.0', '1');
 insert into Items(Items_Name, Items_Price, CategoryID_FK) values ('Pickles', '5.0', '3');
 insert into Items(Items_Name, Items_Price, CategoryID_FK) values ('Ice Tea', '3.0', '5');
 insert into Items(Items_Name, Items_Price, CategoryID_FK) values ('Soda', '10.0', '5');
 
 /*insert into Invoices(Invoices_Date, TableID_FK, Invoices_Status) values(current_timestamp(), 1, 1);
 insert into Invoices(Invoices_Date, TableID_FK, Invoices_Status) values(current_timestamp(), 2, 2);
 insert into Invoices(Invoices_Date, TableID_FK, Invoices_Status) values(current_timestamp(), 3, 3);
 
 insert into Invoice_details(InvoicesID_FK, ItemsID_FK, Items_Price, count) values (1, 2, 10.0, 3);
 insert into Invoice_details(InvoicesID_FK, ItemsID_FK, Items_Price, count) values (1, 5, 15.0, 2);
 insert into Invoice_details(InvoicesID_FK, ItemsID_FK, Items_Price, count) values (2, 3, 10.0,1);
 insert into Invoice_details(InvoicesID_FK, ItemsID_FK, Items_Price, count) values (2, 6, 15.0,3);*/
 
 select * from TableFood;
 select TableID_FK from Invoices where Invoices_ID = 1;
 update TableFood set Tables_Status = 1 where Tables_ID = 4;
 
 select * from Invoices;
 select * from Invoices where TableID_FK = 1;
 select * from Invoices where TableID_FK = 2;
 update Invoices set Invoices_Status = 1 where Invoices_ID = 1;
 select i.Invoices_ID, i.Invoices_Date, c.Tables_Name, i.Invoices_Status from
 Invoices i inner join TableFood c on i.TableID_FK = c.Tables_ID where i.Invoices_Status = 2;
 select i.Invoices_ID, i.Invoices_Date, c.Tables_Name, i.Invoices_Status from
Invoices i inner join TableFood c on i.TableID_FK = c.Tables_ID where i.Invoices_Status > 1;
 
 select * from TableFood;
 
 select * from Invoice_details;
 select * from Invoice_details where InvoicesID_FK = 1;
 select c.Items_Name, c.Items_Price, i.count from 
 Invoice_details i inner join Items c on i.ItemsID_FK = c.Items_ID where i.InvoicesID_FK = 1;
 
 select * from Category;
 
 -- Item - category
 select i.Items_ID, i.Items_Name, i.Items_Price, c.Category_Name from
 Items i inner join Category c on i.CategoryID_FK = c.Category_ID;
 -- ItemID - category
select i.Items_ID, i.Items_Name, i.Items_Price, c.Category_Name from
Items i inner join Category c on i.CategoryID_FK = c.Category_ID where i.Items_ID = 2;
-- ItemName -- category
-- Invoice - table
select i.Invoices_ID, i.Invoices_Date, c.Tables_Name, i.Invoices_Status from
 Invoices i inner join TableFood c on i.TableID_FK = c.Tables_ID;
 -- InvoiceID - table
select i.Invoices_ID, i.Invoices_Date, c.Tables_Name, i.Invoices_Status from
 Invoices i inner join TableFood c on i.TableID_FK = c.Tables_ID where i.TableID_FK = 1;
 -- InvoiceID - info
select i.Invoices_ID, i.Invoices_Date, c.Tables_Name, i.Invoices_Status from
Invoices i inner join TableFood c on i.TableID_FK = c.Tables_ID where i.Invoices_Status = 1 and i.Invoices_ID = 1;
 
 -- Create table
 delimiter $$
create procedure sp_createTable(IN tableName varchar(100),  OUT tableId int)
begin
	insert into TableFood(Tables_Name) values (tableName); 
    select max(Tables_ID) into tableId from TableFood;
end $$
delimiter ;

-- Create Item
delimiter $$
create procedure sp_createItem(IN itemName varchar(100), IN itemPrice decimal(20,3), IN categoryId int,OUT itemId int)
begin
	insert into Items(Items_Name, Items_Price, CategoryID_FK) values (itemName, itemPrice, categoryId); 
    select max(Items_ID)  into itemId from Items;
end $$
delimiter ;

select i.Items_ID, i.Items_Name, i.Items_Price, c.Category_Name from Items i inner join Category c on i.CategoryID_FK = c.Category_ID
where Items_Name like concat('%',@Items_Name,'%');

select LAST_INSERT_ID();

update TableFood set Tables_Status = 2 where Tables_ID =1;
select * from TableFood;
 
 select * from TableFood where Tables_Status = 2;
select * from Staffs where Username='staffpf13' and Userpass='04d4b37015f6ba05077ae49776a76b95';