
using System;
using System.Collections.Generic;
using System.Text;

namespace Jugueteria.Modelo
{
    public class Producto
    {
        public int producto_id { get; set; }
        public string producto_codigo { get; set; }
        public string producto_nombre { get; set; }
        public decimal producto_precio_compra { get; set; }
        public decimal producto_precio_venta { get; set; }
        public int categoria_id { get; set; }
    }
}
