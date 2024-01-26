namespace EcoRide.Models
{
    public class DetalleRenta
    {
        public int Id { get; set; }
        public int IdRenta { get; set; }
        public int Renglon { get; set; }
        public int IdScooter { get; set; }
        public int Cantidad { get; set; }
        public decimal Tiempo { get; set; }
        public DateTime Fecha { get; set; }
        public DateTime RentaTerminada { get; set; }
        public int MinutosExtra { get; set; }
        public decimal CargoPorExtra { get; set; }
        public decimal SubTotal { get; set; }
    }
}
