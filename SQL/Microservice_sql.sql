create table transaction_logs (
	t_id int auto_increment primary key not null,
    t_time datetime default current_timestamp,
    t_type varchar(100),
    t_account varchar(100),
    t_price float,
    t_quantity int,
	username varchar(50)
);

create table buy (
	b_id int auto_increment primary key not null,
    username varchar(50),
    t_account varchar(100),
	price float,
    quantity int
);

create table sell (
	s_id int auto_increment primary key not null,
    username varchar(50),
    t_account varchar(100),
	price float,
    quantity int
);
