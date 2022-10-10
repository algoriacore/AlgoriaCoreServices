# AlgoriaCore.WebUI
Proyecto con la capa de presentación
- Controllers

# Objetivo
- La capa de presentación NO debe depender en Core y en Infraestructura
- La capa de presentación DEBE depender solo de la capa de aplicación
- DEBE depender solo de la capa (proyecto) Application y del proyecto Infrastructure

# DATABASE CONFIGURATION
- Se deben configurar la o las conexiones a usar "AlgoriaCoreDatabase" (SQL Server, default) y "MySqlAlgoriaCoreDatabase" (MySql)
- Configurar el parámetro "AppSettings.DatabaseType":
    * Valor "0" -> SQL Server (default)
    * Valor "1" -> MySql