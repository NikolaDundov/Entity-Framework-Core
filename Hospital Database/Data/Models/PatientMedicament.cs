namespace P01_HospitalDatabase.Data.Models
{
    public class PatientMedicament
    {
        public int PatientId { get; set; }

        public Patient Patient { get; set; }

        public int MedicmentId { get; set; }

        public Medicament Medicament { get; set; }
    }
}
