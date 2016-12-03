using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Xml.Linq;
using SharedLibs.DataContracts;

namespace ServiceBus
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "CER0285Service" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select CER0285Service.svc or CER0285Service.svc.cs at the Solution Explorer and start debugging.
    public class CER0285Service : ICER0285Service
    {

        public Student GetStudent(Guid guid)
        {
            var xml = XDocument.Load("~/App_Data/testfile.xml");

            var query = from c in xml.Root.Descendants("catalog")
                where c.Name == "Martin"
                select c.Element("name").Value;

            SharedLibs.DataContracts.Student s = new Student();

            foreach (string x in query)
            {
                s.Name = x;
            }

            return new SharedLibs.DataContracts.Student() {};
        }

        public List<Student> GetAllStudents()
        {
            var list = new List<Student>();

            //TODO projit query foreach a vratit zaznamy

            return list;
        }

        public List<Student> GetAllStudentsSorted()
        {
            var list = new List<Student>();

            //TODO projit query kde bude order by a pak foreach a vratit zaznamy

            return list;
        }

        public List<Student> GetStudentsByCity(string cityname)
        {
            var list = new List<Student>();

            //TODO projit query a klauze where city is not null foreach a vratit zaznamy

            return list;
        }

        public Result SendStudentsByEmail()
        {
            try
            {
                MailMessage mail = new MailMessage(new MailAddress("cer0285@vsb.cz"), new MailAddress("cer0285@vsb.cz"));
                mail.IsBodyHtml = true;

                string html = "...";
                //TODO presypat tu kolekci

                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(html, null,
                    MediaTypeNames.Text.Html);

                mail.AlternateViews.Add(htmlView);

                mail.Subject = "skola test";

                SmtpClient client = new SmtpClient("smtp.vsb.cz");
                client.Send(mail);

                return Result.SuccessFormat("Yay !");
            }
            catch (Exception e)
            {
                return Result.FatalFormat("Mail was not sent because an error has occured {0}",e.Message);
            }
        }
    }
}
