CREATE OR ALTER PROCEDURE SP_GUARDAR_PERMISOS_POR_ROL
(
    @IdRol INT,
    @ListaAcciones NVARCHAR(MAX),
    @ListaSubMenus NVARCHAR(MAX),  -- ej: '5,6'
    @ListaModulos NVARCHAR(MAX)    -- ej: '10'
)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Acciones TABLE (IDACCION INT);
    DECLARE @SubMenus TABLE (IDSUBMENU INT);
    DECLARE @Modulos TABLE (IDMODULO INT);

    -- Convertir cadenas a tablas
    IF LEN(@ListaAcciones) > 0
    BEGIN
        DECLARE @Xml XML = '<i>' + REPLACE(@ListaAcciones, ',', '</i><i>') + '</i>';
        INSERT INTO @Acciones (IDACCION)
        SELECT t.value('.', 'INT') FROM @Xml.nodes('//i') AS x(t);
    END

    IF LEN(@ListaSubMenus) > 0
    BEGIN
        DECLARE @XmlSub XML = '<i>' + REPLACE(@ListaSubMenus, ',', '</i><i>') + '</i>';
        INSERT INTO @SubMenus (IDSUBMENU)
        SELECT t.value('.', 'INT') FROM @XmlSub.nodes('//i') AS x(t);
    END

    IF LEN(@ListaModulos) > 0
    BEGIN
        DECLARE @XmlMod XML = '<i>' + REPLACE(@ListaModulos, ',', '</i><i>') + '</i>';
        INSERT INTO @Modulos (IDMODULO)
        SELECT t.value('.', 'INT') FROM @XmlMod.nodes('//i') AS x(t);
    END

    BEGIN TRY
        BEGIN TRANSACTION;

        -- 1?? Eliminar solo acciones que ya no están seleccionadas
        DELETE FROM PERMISO
        WHERE IdRol = @IdRol
        AND IDACCION IS NOT NULL
        AND IDACCION NOT IN (SELECT IDACCION FROM @Acciones);

        -- 2?? Insertar acciones seleccionadas
        INSERT INTO PERMISO (IdRol, IDMODULO, IDSUBMENU, IDACCION, FechaAsignacion)
        SELECT 
            @IdRol,
            sm.IDMODULO,
            a.IDSUBMENU,
            a.IDACCION,
            GETDATE()
        FROM ACCION a
        INNER JOIN SUBMENU sm ON sm.IDSUBMENU = a.IDSUBMENU
        WHERE a.IDACCION IN (SELECT IDACCION FROM @Acciones)
        AND NOT EXISTS (
            SELECT 1 FROM PERMISO p
            WHERE p.IdRol = @IdRol AND p.IDACCION = a.IDACCION
        );

        -- 3?? Insertar submenus sin acciones
        INSERT INTO PERMISO (IdRol, IDMODULO, IDSUBMENU, IDACCION, FechaAsignacion)
        SELECT 
            @IdRol,
            sm.IDMODULO,
            sm.IDSUBMENU,
            NULL,
            GETDATE()
        FROM SUBMENU sm
        INNER JOIN @SubMenus s ON s.IDSUBMENU = sm.IDSUBMENU
        WHERE NOT EXISTS (
            SELECT 1 FROM PERMISO p
            WHERE p.IdRol = @IdRol AND p.IDSUBMENU = sm.IDSUBMENU AND p.IDACCION IS NULL
        );

        -- 4?? Insertar módulos sin submenus ni acciones
        INSERT INTO PERMISO (IdRol, IDMODULO, IDSUBMENU, IDACCION, FechaAsignacion)
        SELECT 
            @IdRol,
            m.IDMODULO,
            NULL,
            NULL,
            GETDATE()
        FROM MODULO m
        INNER JOIN @Modulos mo ON mo.IDMODULO = m.IDMODULO
        WHERE NOT EXISTS (
            SELECT 1 FROM PERMISO p
            WHERE p.IdRol = @IdRol AND p.IDMODULO = m.IDMODULO AND p.IDSUBMENU IS NULL AND p.IDACCION IS NULL
        );

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE SP_LISTAR_MODULOS_SUBMENU_ACCIONES
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        m.IDMODULO,
        m.NOMBREMODULO,
        s.IDSUBMENU,
        s.NOMBRESUBMENU,
        a.IDACCION,
        a.NOMBREACCION
    FROM MODULO m
    LEFT JOIN SUBMENU s ON s.IDMODULO = m.IDMODULO
    LEFT JOIN ACCION a ON a.IDSUBMENU = s.IDSUBMENU
    ORDER BY 
        m.NOMBREMODULO,
        s.NOMBRESUBMENU,
        a.NOMBREACCION;
END;
GO


CREATE OR ALTER PROCEDURE SP_LISTAR_PERMISOS_POR_USUARIO
    @IdUsuario INT
AS
BEGIN
    SELECT 
        p.IdPermiso,
        p.IdRol,
        r.Nombre AS Rol,
        p.IdModulo,
        m.NombreModulo,
        p.IdSubMenu,
        s.NombreSubMenu,
        p.IdAccion,
        a.NombreAccion
    FROM PERMISO p
    INNER JOIN ROL r ON r.IdRol = p.IdRol
    LEFT JOIN MODULO m ON m.IdModulo = p.IdModulo
    LEFT JOIN SUBMENU s ON s.IdSubMenu = p.IdSubMenu
    LEFT JOIN ACCION a ON a.IdAccion = p.IdAccion
    INNER JOIN USUARIO u ON u.IdRol = r.IdRol
    WHERE u.IdUsuario = @IdUsuario
END
GO

CREATE OR ALTER PROCEDURE SP_LISTAR_ESTRUCTURA_PO_USUARIO
(
    @IdUsuario INT
)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT DISTINCT 
        M.IDMODULO, M.NOMBREMODULO,
        S.IDSUBMENU, S.NOMBRESUBMENU,
        A.IDACCION, A.NOMBREACCION
    FROM PERMISO P
    INNER JOIN MODULO M ON P.IDMODULO = M.IDMODULO
    LEFT JOIN SUBMENU S ON P.IDSUBMENU = S.IDSUBMENU
    LEFT JOIN ACCION A ON P.IDACCION = A.IDACCION
    INNER JOIN ROL R ON P.IDROL = R.IDROL
    INNER JOIN USUARIO U ON U.IDROL = R.IDROL
    WHERE U.IDUSUARIO = @IdUsuario
    ORDER BY M.IDMODULO, S.IDSUBMENU, A.IDACCION;
END
GO



CREATE OR ALTER PROCEDURE SP_ObtenerPermisos
AS
BEGIN
    SELECT P.IDPERMISO, P.IDROL, R.NOMBRE AS ROLNOMBRE,
           P.IDMODULO, M.NOMBREMODULO, 
           P.IDSUBMENU, S.NOMBRESUBMENU,
           P.IDACCION, A.NOMBREACCION,
           P.FECHAASIGNACION
    FROM PERMISO P
    INNER JOIN ROL R ON P.IDROL = R.IDROL
    LEFT JOIN MODULO M ON P.IDMODULO = M.IDMODULO
    LEFT JOIN SUBMENU S ON P.IDSUBMENU = S.IDSUBMENU
    LEFT JOIN ACCION A ON P.IDACCION = A.IDACCION
END
GO


CREATE OR ALTER PROCEDURE SP_LISTAR_PERMISOS_POR_ROL
(
    @IdRol INT
)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT DISTINCT 
        M.IDMODULO,
        M.NOMBREMODULO AS NombreModulo,
        S.IDSUBMENU,
        S.NOMBRESUBMENU AS NombreSubMenu,
        A.IDACCION,
        A.NOMBREACCION AS NombreAccion
    FROM PERMISO P
    INNER JOIN MODULO M ON P.IDMODULO = M.IDMODULO
    LEFT JOIN SUBMENU S ON P.IDSUBMENU = S.IDSUBMENU
    LEFT JOIN ACCION A ON P.IDACCION = A.IDACCION
    WHERE P.IDROL = @IdRol
    ORDER BY M.NOMBREMODULO, S.NOMBRESUBMENU;
END

