using NMBDHL.Patient.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMBDHL.Patient.Repository
{
    public interface IPatientRepository
    {
        Task<bool> RegisterPatientAsync(
            PatientRegistration patient,
            string passwordHash,
            string homeAddress);
    }
}
