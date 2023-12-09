using AlgoriaCore.Application.BaseClases.Dto;
using AlgoriaCore.Application.Managers.Base;
using AlgoriaCore.Application.Managers.Catalogos.CatalogosDinamicos.Dto;
using AlgoriaCore.Domain.Entities;
using AlgoriaCore.Extensions;
using AlgoriaPersistence.Interfaces.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.Managers.Catalogos.CatalogosDinamicos
{
    public class DynamicCatalogsManager : BaseManager
    {
        private readonly IRepository<CatalogoDinamico, long> _repCatalogoDinamico;
        private readonly IRepository<CatalogoDinamicoDefinicion, long> _repCatalogoDinamicoDef;
        private readonly IRepository<CatalogoDinamicoRelacion, long> _repCatalogoDinamicoRel;
        private readonly IRepository<CatalogoDinamicoValidacion, long> _repCatalogoDinamicoVal;

        private readonly ISqlExecuter _sqlExecuter;

        public DynamicCatalogsManager(IRepository<CatalogoDinamico, long> repCatalogoDinamico,
            IRepository<CatalogoDinamicoDefinicion, long> repCatalogoDinamicoDef,
            IRepository<CatalogoDinamicoRelacion, long> repCatalogoDinamicoRel,
            IRepository<CatalogoDinamicoValidacion, long> repCatalogoDinamicoVal,
            ISqlExecuter sqlExecuter
            )
        {
            _repCatalogoDinamico = repCatalogoDinamico;
            _repCatalogoDinamicoDef = repCatalogoDinamicoDef;
            _repCatalogoDinamicoRel = repCatalogoDinamicoRel;
            _repCatalogoDinamicoVal = repCatalogoDinamicoVal;
            _sqlExecuter = sqlExecuter;
        }

        public async Task<List<ComboboxItemDto>> GetCatalogosDinamicosListAsync()
        {
            var ll = await (from c in _repCatalogoDinamico.GetAll()
                      where c.IsActive == true
                      select new ComboboxItemDto
                      {
                          Value = c.Tabla,
                          Label = c.Descripcion
                      }).ToListAsync();

            return ll;
        }

        public async Task<DynamicCatalogViewInfoDto> GetCatalogoDinamicoVistaDefinicionesList(string nombreTabla)
        {
            DynamicCatalogViewInfoDto respuesta = new DynamicCatalogViewInfoDto();

            var d = GetCatalogoDinamicoMetadatos(nombreTabla);
            respuesta.CampoLlavePrimaria = d.CampoLlavePrimaria;
            respuesta.DefinicionesList = GetCatalogoDinamicoMetadatosDefiniciones(d.Id);
            respuesta.CamposVistaList = GetCamposConsultaSql(d).Values.ToList();
            respuesta.RelationsList = GetCatalogoDinamicoMetadatosRelaciones(d.Id);

            return respuesta;
        }

        public async Task<DynamicCatalogFormInfoDto> GetCatalogoDinamicoFormDefinicionesList(string nombreTabla)
        {
            DynamicCatalogFormInfoDto respuesta = new DynamicCatalogFormInfoDto();

            var d = GetCatalogoDinamicoMetadatos(nombreTabla);
            respuesta.CampoLlavePrimaria = d.CampoLlavePrimaria;
            respuesta.DefinitionsList = GetCatalogoDinamicoMetadatosDefiniciones(d.Id).Where(m => m.CapturarEnPantalla).ToList();
            respuesta.ValidationsList = GetCatalogoDinamicoMetadatosValidaciones(d.Id);
            respuesta.RelationsList = GetCatalogoDinamicoMetadatosRelaciones(d.Id);

            return respuesta;
        }

        public async Task<List<Dictionary<string,object>>> GetList(DynamicCatalogListFilterDto filter)
        {
            // Obtiene información de metadatos principal del catálogo solicitado
            var d = GetCatalogoDinamicoMetadatos(filter.Tabla);

            var sqlQuery = GetConsultaSQLParaObtenerLista(d, filter);

            var parameters = new Dictionary<string, object>();
            parameters.Add("@strSelectTabla", sqlQuery);
            parameters.Add("@numeropagina", filter.PageNumber);
            parameters.Add("@porpagina", filter.PageSize);
            parameters.Add("@orderby", filter.Sorting);

            var results = await _sqlExecuter.SqlStoredProcedureToDictionary("spr_obtienedatosporseccion", parameters);

            return results;
        }

        public async Task<Dictionary<string, object>> GetById(string nombreTabla, string id)
        {
            Dictionary<string, object> resp = null;

            // Obtiene información de metadatos principal del catálogo solicitado
            var d = GetCatalogoDinamicoMetadatos(nombreTabla);
            var sql = GetConsultaSQLParaObtenerPorId(d, id);

            var results = await _sqlExecuter.SqlQueryToDictionary(sql);

            if (results.Count > 0)
            {
                resp = results[0];
            }

            return resp;
        }

        public async Task CreateRegistroAsync(string nombreTabla, Dictionary<string, object> pars)
        {
            var d = GetCatalogoDinamicoMetadatos(nombreTabla);

            var sql = GetConsultaParaInsert(d);

            var definiciones = GetCatalogoDinamicoMetadatosDefiniciones(d.Id).Where(m => m.MostrarEnVista).ToList();

            Dictionary<string, object> aParameters = new Dictionary<string, object>();
            foreach (var k in definiciones)
            {
                if (k.Campo.ToLower() != "id" && k.Campo.ToLower() != "tenantid")
                {
                    aParameters.Add("@" + k.Campo, pars[k.Campo]);
                }
            }

            // Se agrega el parámetro tenantId
            if (d.TieneTenantId)
            {
                aParameters.Add("@TenantId", CurrentUnitOfWork.GetTenantId());
            }

            if (d.TieneIsDeleted)
            {
                aParameters.Add("@IsDeleted", 0);
            }

            var resp = await _sqlExecuter.ExecuteSqlCommandAsync(sql, aParameters);
        }

        public async Task UpdateRegistroAsync(string nombreTabla, Dictionary<string, object> pars)
        {
            var d = GetCatalogoDinamicoMetadatos(nombreTabla);

            var sql = GetConsultaParaUpdate(d, pars["Id"].ToString());

            var definiciones = GetCatalogoDinamicoMetadatosDefiniciones(d.Id).Where(m=>m.CapturarEnPantalla).ToList();

            Dictionary<string, object> aParameters = new Dictionary<string, object>();
            foreach (var k in definiciones)
            {
                if (k.Campo.ToLower() != "id" && k.Campo.ToLower() != "isdeleted")
                {
                    aParameters.Add("@" + k.Campo, pars[k.Campo]);
                }
            }

            // Se agrega el parámetro tenantId
            if (d.TieneTenantId)
            {
                // El tenant Id se agregó cuando se generó la consulta de update..
            }

            var resp = await _sqlExecuter.ExecuteSqlCommandAsync(sql, aParameters);
        }

        public async Task DeleteRegistroAsync(string nombreTabla, string id)
        {
            var d = GetCatalogoDinamicoMetadatos(nombreTabla);
            var sql = GetConsultaParaDelete(d, id);

            var resp = await _sqlExecuter.ExecuteSqlCommandAsync(sql, new Dictionary<string, object>());
        }

        #region Métodos privados

        private string GetConsultaSQLParaObtenerLista(DynamicCatalogDto dto, DynamicCatalogListFilterDto filtro)
        {
            var sql = GetConsultaSqlBase(dto);

            var camposConsulta = GetCamposConsultaSql(dto);

            // Agregar filtros a la consulta
            var sqlFilter = string.Empty;
            if (!filtro.Filter.IsNullOrEmpty())
            {
                foreach (var c in camposConsulta)
                {
                    sqlFilter += (sqlFilter.IsNullOrEmpty() ? " WHERE (" : " OR ") + string.Format("{0} LIKE '%{1}%'", c.Key, filtro.Filter);
                }

                sqlFilter += ")";
            }

            // Si tiene tenantId, entonces hay que aagregar el filtro del tenantid
            if (dto.TieneTenantId)
            {
                var tId = CurrentUnitOfWork.GetTenantId();

                sqlFilter += (sqlFilter.IsNullOrEmpty() ? " WHERE " : " AND ") + (tId.HasValue ? string.Format("tenantid='{0}'", tId.Value) : "tenantid is null");
            }

            if (dto.TieneIsDeleted)
            {
                sqlFilter += (sqlFilter.IsNullOrEmpty() ? " WHERE " : " AND ") + (string.Format("(isdeleted is null OR isdeleted=0)"));
            }

            sql = sql + sqlFilter;

            return sql;
        }

        private string GetConsultaSQLParaObtenerPorId(DynamicCatalogDto dto, string id)
        {
            var sql = GetConsultaSqlBase(dto);

            string sqlFilter = string.Format(" WHERE id = '{0}'", id); ;

            // Si tiene tenantId, entonces hay que aagregar el filtro del tenantid
            if (dto.TieneTenantId)
            {
                var tId = CurrentUnitOfWork.GetTenantId();

                sqlFilter += (sqlFilter.IsNullOrEmpty() ? " WHERE " : " AND ") + (tId.HasValue ? string.Format("tenantid={0}", tId.Value) : "tenantid is null");
            }

            if (dto.TieneIsDeleted)
            {
                sqlFilter += (sqlFilter.IsNullOrEmpty() ? " WHERE " : " AND ") + ("(isdeleted is null OR isdeleted=0)");
            }

            sql = sql + sqlFilter;

            return sql;
        }

        private string GetConsultaSQLParaObtenerDuplicados(DynamicCatalogDto dto, Dictionary<string, string> filtros)
        {
            var sql = GetConsultaSqlBase(dto);

            // Agregar filtros a la consulta
            var sqlFilter = string.Empty;
            foreach (var c in filtros)
            {
                sqlFilter += (sqlFilter.IsNullOrEmpty() ? " WHERE (" : " AND ") + string.Format("{0} = '{1}'", c.Key, c.Value);
            }

            if (!sqlFilter.IsNullOrEmpty())
            {
                sqlFilter += ")";
            }

            // Si tiene tenantId, entonces hay que aagregar el filtro del tenantid
            if (dto.TieneTenantId)
            {
                var tId = CurrentUnitOfWork.GetTenantId();

                sqlFilter += (sqlFilter.IsNullOrEmpty() ? " WHERE " : " AND ") + (tId.HasValue ? string.Format("tenantid='{0}'", tId.Value) : "tenantid is null");
            }

            if (dto.TieneIsDeleted)
            {
                sqlFilter += (sqlFilter.IsNullOrEmpty() ? " WHERE " : " AND ") + ("(isdeleted is null OR isdeleted=0)");
            }

            sql = sql + sqlFilter;

            return sql;
        }

        private string GetConsultaSqlBase(DynamicCatalogDto dto)
        {
            // Este método formará una consulta SQL cruda con los campos definidos en la tabla CatalogoDinamicoDefinicion 
            // para el id de catálogo dinámico solicitado..
            var definiciones = GetCatalogoDinamicoMetadatosDefiniciones(dto.Id).Where(m => m.MostrarEnVista).ToList();
            var relaciones = GetCatalogoDinamicoMetadatosRelaciones(dto.Id);

            // Construir la lista de campos a mostrar en la consulta
            var camposConsulta = GetCamposConsultaSql(dto);
            string camposQuery = string.Empty;
            foreach (var k in camposConsulta)
            {
                camposQuery += (camposQuery.IsNullOrEmpty() ? "" : ",") + k.Key + " AS " + k.Value;
            }

            string leftJoins = string.Empty;
            var campos = definiciones.Where(m => m.MostrarEnVista).ToList();

            foreach (var c in campos)
            {
                var cRel = relaciones.FirstOrDefault(m => m.CampoRelacion == c.Campo);
                if (cRel != null)
                {
                    // Construir leftjoins
                    leftJoins += string.Format(" LEFT JOIN {0} ON {1}.{2} = {3}.{4} ", cRel.TablaRelacionada, dto.Tabla, c.Campo, cRel.TablaRelacionada, cRel.CampoReferenciado);
                }
            }

            var sql = string.Format("SELECT {0} FROM {1} {2}", camposQuery, dto.Tabla, leftJoins);

            return sql;
        }

        private string GetConsultaParaInsert(DynamicCatalogDto dto)
        {
            // Este método formará una consulta SQL cruda con los campos definidos en la tabla CatalogoDinamicoDefinicion 
            // para el id de catálogo dinámico solicitado..
            var definiciones = GetCatalogoDinamicoMetadatosDefiniciones(dto.Id).ToList();

            // Construir la lista de campos a mostrar en la consulta
            string camposInsert = string.Empty;
            string paramsInsert = string.Empty;
            foreach (var k in definiciones)
            {
                if (k.Campo.ToLower() != "id")
                {
                    camposInsert += (camposInsert.IsNullOrEmpty() ? "" : ",") + k.Campo;
                    paramsInsert += (paramsInsert.IsNullOrEmpty() ? "@" : ",@") + k.Campo;
                }
            }

            var sql = string.Format("INSERT INTO {0} ({1}) VALUES ({2})", dto.Tabla, camposInsert, paramsInsert);

            return sql;
        }

        private string GetConsultaParaUpdate(DynamicCatalogDto dto, string id)
        {
            // Este método formará una consulta SQL cruda con los campos definidos en la tabla CatalogoDinamicoDefinicion 
            // para el id de catálogo dinámico solicitado..
            var definiciones = GetCatalogoDinamicoMetadatosDefiniciones(dto.Id).Where(m => m.CapturarEnPantalla).ToList();

            // Construir la lista de campos 
            string paramsUpdate = string.Empty;
            foreach (var k in definiciones)
            {
                if (k.Campo.ToLower() != "id" && k.Campo.ToLower() != "isdeleted")
                {
                    paramsUpdate += (paramsUpdate.IsNullOrEmpty() ? "" : ",") + (k.Campo + "=@" + k.Campo);
                }
            }

            var sql = string.Format("UPDATE {0} SET {1} WHERE id='{2}'", dto.Tabla, paramsUpdate, id);

            // Si tiene tenantId, entonces hay que aagregar el filtro del tenantid
            if (dto.TieneTenantId)
            {
                var tId = CurrentUnitOfWork.GetTenantId();

                sql += " AND " + (tId.HasValue ? string.Format("tenantid='{0}'", tId.Value) : "tenantid is null");
            }

            return sql;
        }

        private string GetConsultaParaDelete(DynamicCatalogDto dto, string id)
        {
            var definiciones = GetCatalogoDinamicoMetadatosDefiniciones(dto.Id).Where(m => m.CapturarEnPantalla).ToList();
            var sql = string.Empty;
            if (dto.TieneIsDeleted)
            {
                sql = string.Format("UPDATE {0} SET IsDeleted=1 WHERE {1}='{2}", dto.Tabla, dto.CampoLlavePrimaria, id);
            }
            else
            {
                sql = string.Format("DELETE FROM {0} WHERE {1}='{2}'", dto.Tabla, dto.CampoLlavePrimaria, id);
            }

            // Si tiene tenantId, entonces hay que agregar el filtro del tenantid
            if (dto.TieneTenantId)
            {
                var tId = CurrentUnitOfWork.GetTenantId();

                sql += " AND " + (tId.HasValue ? string.Format("tenantid='{0}'", tId.Value) : "tenantid is null");
            }

            return sql;
        }

        private Dictionary<string, string> GetCamposConsultaSql(DynamicCatalogDto dto)
        {
            Dictionary<string, string> resp = new Dictionary<string, string>();

            // Este método formará una consulta SQL cruda con los campos definidos en la tabla CatalogoDinamicoDefinicion 
            // para el id de catálogo dinámico solicitado..
            var definiciones = GetCatalogoDinamicoMetadatosDefiniciones(dto.Id).ToList();
            var relaciones = GetCatalogoDinamicoMetadatosRelaciones(dto.Id);

            // Construir la lista de campos a mostrar en la consulta
            var campos = definiciones.Where(m => m.MostrarEnVista).ToList();

            foreach (var c in campos)
            {
                // Si el campo actual existe en una relación, entonces se debe agregar el campo "descripcion" de la relación..
                var cRel = relaciones.FirstOrDefault(m => m.CampoRelacion == c.Campo);
                if (cRel != null)
                {
                    resp.Add(cRel.TablaRelacionada + "." + cRel.CampoDescReferenciado, cRel.TablaRelacionada + cRel.CampoDescReferenciado);
                }
                else
                {
                    // Si no existe en una relación, entonces se agrega el campo como tal
                    resp.Add(dto.Tabla + "." + c.Campo, c.Campo);
                }
            }

            return resp;
        }

        private DynamicCatalogDto GetCatalogoDinamicoMetadatos(string nombreTabla)
        {
            var ll = (from c in _repCatalogoDinamico.GetAll()
                      where c.Tabla == nombreTabla
                      select new DynamicCatalogDto
                      {
                          Id = c.Id,
                          Tabla = c.Tabla,
                          Descripcion = c.Descripcion,
                          MetadatosGenerados = c.MetadatosGenerados ?? false,
                          IsActive = c.IsActive ?? false
                      }).FirstOrDefault();

            if (ll == null)
            {
                throw new Exception("Catálogo no encontrado");
            }

            if (!ll.IsActive)
            {
                throw new Exception("El catálogo no está activo. No se puede administrar.");
            }

            if (!ll.MetadatosGenerados)
            {
                // Hay que ejecutar esta línea para que se generen los metadatos del catálogo
                var parameters = new Dictionary<string, object>();
                parameters.Add("@nombreTabla", nombreTabla);
                var r = _sqlExecuter.ExecuteStoredProcedureScalar<long>("EXEC spr_catalogodinamico_generarmetadatos {0}", parameters);
            }

            var campos = (from c in _repCatalogoDinamicoDef.GetAll()
                          where c.CatalogoDinamicoId == ll.Id
                          select c).ToList();

            ll.TieneTenantId = campos.FirstOrDefault(m => m.Campo.ToLower() == "tenantid") != null;
            ll.TieneIsDeleted = campos.FirstOrDefault(m => m.Campo.ToLower() == "isdeleted") != null;

            var llavePrimaria = GetCatalogoDinamicoMetadatosDefiniciones(ll.Id).FirstOrDefault(m => m.EsLlavePrimaria);
            if (llavePrimaria != null)
            {
                ll.CampoLlavePrimaria = llavePrimaria.Campo;
            }

            return ll;
        }

        private List<DynamicCatalogDefinitionDto> GetCatalogoDinamicoMetadatosDefiniciones(long id)
        {
            var ll = (from c in _repCatalogoDinamicoDef.GetAll()
                      where c.CatalogoDinamicoId == id
                      select new DynamicCatalogDefinitionDto
                      {
                          Id = c.Id,
                          Campo = c.Campo,
                          Tipo = c.Tipo,
                          Longitud = c.Longitud,
                          Decimales = c.Decimales,
                          MostrarEnVista = c.MostrarEnVista ?? false,
                          CapturarEnPantalla = c.CapturarEnPantalla ?? false,
                          EsLlavePrimaria = c.EsLlavePrimaria ?? false,
                          Posicion = c.Posicion ?? 0
                      }).ToList();

            return ll;
        }

        private List<DynamicCatalogRelationDto> GetCatalogoDinamicoMetadatosRelaciones(long id)
        {
            var ll = (from c in _repCatalogoDinamicoRel.GetAll()
                      where c.CatalogoDinamicoId == id
                      select new DynamicCatalogRelationDto
                      {
                          Id = c.Id,
                          CampoRelacion = c.CampoRelacion,
                          TablaRelacionada = c.TablaReferenciada,
                          CampoReferenciado = c.CampoReferenciado,
                          CampoDescReferenciado = c.CampoDescReferenciado,
                          TablaReferenciadaTieneIsActive = c.TablaRefTieneIsActive ?? false,
                          TablaReferenciadaTieneIsDeleted = c.TablaRefTieneIsDeleted ?? false
                      }).ToList();

            return ll;
        }

        private List<DynamicCatalogValidationDto> GetCatalogoDinamicoMetadatosValidaciones(long id)
        {
            var ll = (from c in _repCatalogoDinamicoVal.GetAll()
                      where c.CatalogoDinamicoId == id
                      select new DynamicCatalogValidationDto
                      {
                          Id = c.Id,
                          Campo = c.Campo,
                          Regla = c.Regla,
                          ValorReferencia = c.ValorReferencia
                      }).ToList();

            return ll;
        }

        #endregion
    }
}
