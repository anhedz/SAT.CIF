***************************************************************************************************
*** Libreria para Cedula de Identificación Fiscal 
*** Versión: 1.0.5 01/04/2023 0104202310
***************************************************************************************************

Emitido por la Secretaría de Hacienda y Crédito Público (SHCP) y en el cual se pueden encontrar 
diferentes datos relativos a determinado contribuyente, con el que se puede llevar una mejor 
gestión de todo el padrón que tiene obligaciones fiscales y tributarias con el estado mexicano. 
En primera instancia, cabe aclarar que se puede definir como un contribuyente a toda aquella 
persona que se encuentra dada de alta ante el Servicio de Administración Tributaria (SAT) y
que hace alguna actividad por la que recibe ingresos.

# Fundamento Legal
Código Fiscal de la Federación, artículo 27.
Resolución Miscelánea Fiscal, regla 2.4.10.

** Revisiones:

02/10/2023
- cambios de namespace

02/05/2023
- se agrega propiedad IdCIF para contener el número de la cedula de identificación
- se agrega Regex para determinar el ID de la cedula fiscal

19/04/2023
- se agrega método Execute(string url), con validación Uri.IsWellFormedUriString()
Execute(string url)
Execute(IRequest request)
Execute(FileInfo fileInfo)

18/04/2023
- Version 1.0.0.3
- se agrega contador a la llave por si se repite o existe mas de un elemento en HtmlToDiccionary.CreateRows(), 
para los casos de las personas fisicas que tienen mas de un régimen.

11/04/2023: 
- Version 1.0.0.2
- bandera para indicar si el servicio debe almacenar la respuesta html
- se agrega log de errores