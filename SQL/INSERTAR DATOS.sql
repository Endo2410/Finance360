INSERT INTO USUARIO (IDROL,IDESTADO,NOMBRES,APELLIDOS,NOMBREUSUARIO,CORREO,CLAVE) VALUES
(1,1,'LUIS','BLANCO','jblanco','jlpezblanco170@gmail.com','jl2410')

UPDATE USUARIO
SET CORREO = 'soporte@farmaciasaba.com'
WHERE IDUSUARIO = 2;  

UPDATE USUARIO
SET CORREO = 'soporte02@farmaciasaba.com'
WHERE IDUSUARIO = 1;  

select * from USUARIO
INSERT INTO ESTADO (NOMBRE) VALUES
('ACTIVO'),
('NO ACTIVO')

INSERT INTO ROL (NOMBRE,DESCRIPCION,IDESTADO) VALUES
('ADMINISTRADOR','ENCARGADO DE MANEJAR EL SISTEMA',1)

INSERT INTO PERMISO(IDROL,IDMODULO,IDSUBMENU) VALUES
(1,2,3)

SELECT * FROM SUBMENU
INSERT INTO MODULO (NombreModulo) VALUES
('Nota_Credito')

('Dashboard'),
('Seguridad'),
('OrdenCompra'),
('Catalogo'),
('Publicidad'),
('Rebate'),
('Vencidos')

SELECT * FROM MODULO

INSERT INTO SUBMENU (IdModulo, NombreSubMenu) VALUES
(1006, 'Lista_Pago_Vencdio'),

(1007, 'Nota_Credito'),
(1006, 'Pago_Vencido'),
(1005, 'Pago_Rebate'),
(1005, 'Lista_Pago_Rebate'),

-- Seguridad
(2, 'Usuarios'),
(2, 'Rol'),
(2, 'Asignar Permisos'),

--orden compra
(3, 'OrdenCompra'),

-- Catálogos
(4, 'Proveedores'),
(4, 'Pais'),
(4, 'Moneda'),
(4, 'Modalidad'),
(4, 'Tipo_Publicidad'),
(4, 'Tipo_Rebate')


-- publicidad
(5, 'Publicidad'),
(5, 'Estado_Cuenta'),
(5, 'Pago_Publicidad'),
(5, 'Lista_Pago'),

-- Rebate
(1005, 'Acuerdo_Rebate'),
(1005, 'Ejecucion_Rebate'),
(1005, 'Pago_Rebate'),
(1005, 'Lista_Pago_Rebate'),

-- Asistencia
(6, 'Listado'),
(6, 'Registrar Entrada/Salida'),
(6, 'Reporte'),
-- Solicitudes
(7, 'Listado'),
(7, 'Nueva Solicitud'),
(7, 'Aprobar / Rechazar'),
-- Nómina
(8, 'Listado'),
(8, 'Generar Nómina'),
(8, 'Periodos'),
-- Vacaciones
(9, 'Listado'),
(9, 'Reporte'),
-- Reportes
(10, 'General'),
(10, 'Vacaciones'),
(10, 'Nómina'),
(10, 'Asistencia'),
(10, 'Empleados Activos');

-- Ejemplo para modalidades existentes
UPDATE MODALIDAD
SET TIPO_INTERVALO = 'MES',    -- DIA, SEMANA, MES, ANIO, etc.
    VALOR_INTERVALO = 1
WHERE NOMBRE = 'MENSUAL';

UPDATE MODALIDAD
SET TIPO_INTERVALO = 'ANIO',
    VALOR_INTERVALO = 1
WHERE NOMBRE = 'Anual';

UPDATE MODALIDAD
SET TIPO_INTERVALO = 'QUINCENA',
    VALOR_INTERVALO = 15
WHERE NOMBRE = 'Quincenal';

UPDATE MODALIDAD
SET TIPO_INTERVALO = 'SEMANA',
    VALOR_INTERVALO = 1
WHERE NOMBRE = 'TRIMESTRAL';

UPDATE MODALIDAD
SET TIPO_INTERVALO = 'DIA',
    VALOR_INTERVALO = 1
WHERE NOMBRE = 'Diario';



INSERT INTO ACCION(IDSUBMENU, NOMBREACCION) VALUES
(1011, 'Pagos Multiples'),
(1006, 'Pago_Vencido')


select * from ACCION
select * from SUBMENU


select * from PERMISO




 select * from ACCION

UPDATE Accion
SET NombreAccion = 'Pagos_Multiples'
WHERE IDACCION = 1 -- tu id de acción


select * from VENCIDOS 
delete from SUBMENU where IDSUBMENU = 1014