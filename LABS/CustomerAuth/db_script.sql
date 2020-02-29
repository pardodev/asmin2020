select * from Users
select * from Roles
select * from UserRoles
insert into UserRoles values(2,1)

insert into Roles values ('System Administrtor')
insert into Users(Username,FirstName,LastName,Email,Password,IsActive,ActivationCode)
select Username = 'admin',FirstName = 'Admin',LastName = 'Sistem',Email = 'pardotkk@yahoo.com',Password='admin123',IsActive = 1,ActivationCode = NEWID()
