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

INSERT INTO administrator (user_id, password, user_name) VALUES ('id4321', 'pass', 'hogehoge');
INSERT INTO administrator (user_id, password, user_name) VALUES ('id1234', 'pass2', 'hoge');
INSERT INTO administrator (user_id, password, user_name) VALUES ('dklsajfl34398', 'pass3', 'foofoo');
