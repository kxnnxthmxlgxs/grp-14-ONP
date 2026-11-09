using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NMBDHL.Patient.Model;
using NMBDHL.DataAccess.SQLDataBaseAccess;

namespace NMBDHL.Patient.Repository
{
    public class PatientRepository:IPatientRepository
    {
        private readonly ISQLDataBaseAccess _db;

        public PatientRepository(ISQLDataBaseAccess db)
        {
            _db = db;
        }

        public async Task<bool> RegisterPatientAsync(
           PatientRegistration patient,
           string passwordHash,
           string homeAddress)
        {
            if (patient == null)
            {
                throw new ArgumentNullException(nameof(patient));
            }

            var result =
                await _db.GetData<PatientRegistrationResult, dynamic>(
                    "sp_Patient_Register",
                    new
                    {
                        Name = patient.Name.Trim(),
                        Surname = patient.Surname.Trim(),

                        IDNumber = string.IsNullOrWhiteSpace(patient.IDNumber)
                            ? null
                            : patient.IDNumber.Trim(),

                        Email = patient.Email.Trim(),

                        ContactNumber = string.IsNullOrWhiteSpace(
                            patient.Cellphone)
                            ? null
                            : patient.Cellphone.Trim(),

                        PasswordHash = passwordHash,
                        DateOfBirth = patient.DateOfBirth?.Date,
                        HomeAddress = homeAddress
                    });

            return result != null &&
                   result.Any(x => x.UserID > 0 && x.PatientID > 0);
        }

        private class PatientRegistrationResult
        {
            public int UserID { get; set; }
            public int PatientID { get; set; }
        }
    }
}

