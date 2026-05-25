TRUNCATE TABLE
    department,
    employee,
    administrator
RESTART IDENTITY CASCADE;

INSERT INTO department (dept_no, dept_name) VALUES (101, '総務部');
INSERT INTO department (dept_no, dept_name) VALUES (102, '経理部');
INSERT INTO department (dept_no, dept_name) VALUES (103, '人事部');
INSERT INTO department (dept_no, dept_name) VALUES (104, '開発部');
INSERT INTO department (dept_no, dept_name) VALUES (105, '営業部');

INSERT INTO employee (emp_name, birthday, mail_address, dept_no) VALUES ('田中太郎', '2003-02-05', 'aaabbbccc1234@gmail.com', 101);
INSERT INTO employee (emp_name, birthday, mail_address, dept_no) VALUES ('鈴木三郎', '2002-03-06', 'hoge@example.com', 102);
INSERT INTO employee (emp_name, birthday, mail_address, dept_no) VALUES ('佐藤花子', '2001-04-07', 'foo89732@ezweb.ne.jp', 103);
INSERT INTO employee (emp_name, birthday, mail_address, dept_no) VALUES ('中田彩子', '2000-05-08', 'cccbbbaaa65536@gmail.com', 104);
INSERT INTO employee (emp_name, birthday, mail_address, dept_no) VALUES ('加藤圭太', '1999-06-09', 'fdssajfljfd21@example.com', 105);
INSERT INTO employee (emp_name, birthday, mail_address, dept_no) VALUES ('松本良太', '1998-07-10', 'woierufd234253@ezweb.ne.jp', null);

INSERT INTO administrator (user_id, password, user_name) VALUES ('id4321', 'pass', 'hogehoge');
INSERT INTO administrator (user_id, password, user_name) VALUES ('id1234', 'pass2', 'hoge');
INSERT INTO administrator (user_id, password, user_name) VALUES ('dklsajfl34398', 'pass3', 'foofoo');
