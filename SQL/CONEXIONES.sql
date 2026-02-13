
--FSCDDB
EXEC sp_addlinkedserver 
   @server='saba',       -- Nombre que le darás al servidor vinculado
   @srvproduct='', 
   @provider='SQLNCLI',           -- Proveedor de SQL Server Native Client
   @datasrc='192.168.222.4';      -- IP o nombre del servidor remoto

EXEC sp_addlinkedsrvlogin
    @rmtsrvname = 'saba', 
    @useself = 'false',
    @rmtuser = 'sa',     -- Usuario de la BD remota
    @rmtpassword = 'Fs1200';   -- Contraseña

--FSHQDB
EXEC sp_addlinkedserver 
   @server='fshqbd_saba',       -- Nombre que le darás al servidor vinculado
   @srvproduct='', 
   @provider='SQLNCLI',           -- Proveedor de SQL Server Native Client
   @datasrc='192.168.222.4';      -- IP o nombre del servidor remoto

   EXEC sp_addlinkedsrvlogin
    @rmtsrvname = 'fshqbd_saba', 
    @useself = 'false',
    @rmtuser = 'sa',     -- Usuario de la BD remota
    @rmtpassword = 'Fs1200';   -- Contraseña
