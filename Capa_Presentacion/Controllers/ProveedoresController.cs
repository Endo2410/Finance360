using Capa_Dato;
using Capa_Entidad;
using Capa_Negocio;
using Capa_Presentacion.Filtros;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Capa_Presentacion.Controllers
{
    //[FiltroSesion]
    public class ProveedoresController : Controller
    {
        private readonly CN_Proveedor objcn = new CN_Proveedor();

        public IActionResult Index()
        {
            List<Proveedor> lista = objcn.ObtenerProveedores();
            return View(lista);
        }

        [HttpGet]
        public IActionResult Listar()
        {
            var lista = objcn.ObtenerProveedores();
            return Json(lista.Select(p => new {
                idProveedor = p.IdProveedor,
                nombre = p.NombreProveedor,
                ruc = p.Ruc
            }));
        }


        [HttpPost]
        public IActionResult Sincronizar()
        {
            var resultado = objcn.SincronizarProveedores();
            return Json(resultado);
        }

        //DEPARTAMENTO 

        public IActionResult Departamento()
        {
            List<Departamento> lista = objcn.ObtenerDepartamentos();
            return View(lista);
        }

        [HttpGet]
        public IActionResult ListarDepartamento()
        {
            var lista = objcn.ObtenerDepartamentos();
            return Json(lista.Select(d => new {
                idDepartamento = d.IdDepartamento,
                nombre = d.NombreDepartamento
            }));
        }


        [HttpPost]
        public IActionResult SincronizarDepartamento()
        {
            var resultado = objcn.SincronizarDepartamentos();
            return Json(resultado);
        }

        //catalogo
        public IActionResult Gestión()
        {
            return View();
        }
    }
}
