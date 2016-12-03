using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.Text;

namespace ServiceBus
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ICER0285Service" in both code and config file together.
    [ServiceContract]
    public interface ICER0285Service
    {
        [OperationContract]
        SharedLibs.DataContracts.Student GetStudent(Guid guid);

        [OperationContract]
        List<SharedLibs.DataContracts.Student> GetAllStudents();

        [OperationContract]
        SharedLibs.DataContracts.Students GetAllStudentsSorted();

        [OperationContract]
        SharedLibs.DataContracts.Students GetStudentsByCity(string cityname);

        [OperationContract]
        SharedLibs.DataContracts.Result SendStudentsByEmail();
    }
}
