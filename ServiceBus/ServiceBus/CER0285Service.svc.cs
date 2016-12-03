using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Xml;
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
            //Ok nacteno. Relativni "~/App_Data/testfile.xml" kdyz zbude cas
            var xml = XDocument.Load(@"D:\Visual Studio workplace\testFork\ServiceBus\ServiceBus\App_Data\testfile.xml");

           

            var query = from item in xml.Descendants("student")
                        where item.Element("id").Value==guid.ToString()
                        select new Student()
                        {
                            Id = Guid.Parse(item.Element("id").Value),
                            Name = item.Element("name").Value,
                            Surename = item.Element("surename").Value,
                            City = item.Element("city").Value,
                            Age = (int)item.Element("age"),
                            Result = Result.Success("ok")
                        };
           
            

            SharedLibs.DataContracts.Student s = new Student();

            foreach (var x in query)
            {//tohle neni idealni ale nejak to nefungovalo jinak
                s.Name = x.Name;
                s.Id = x.Id;
                s.Surename = x.Surename;
                s.City = x.City;
                s.Age = x.Age;
                s.Result = x.Result;
            }
            
            return s;
        }

        public List<Student> GetAllStudents()
        {
            try
            {
                var xml = XDocument.Load(@"D:\Visual Studio workplace\testFork\ServiceBus\ServiceBus\App_Data\testfile.xml");
                var query = from item in xml.Descendants("student")
                            select new Student()
                            {
                                Id = Guid.Parse(item.Element("id").Value),
                                Name = item.Element("name").Value,
                                Surename = item.Element("surename").Value,
                                City = item.Element("city").Value,
                                Age = (int)item.Element("age"),
                                Result = Result.Success("ok")
                            };
                //var list = query.ToList();
                return query.ToList();
            }
            catch (XmlException xmlex)
            {
                //TOhle pouzivam pro email dole uz vracim neco normalniho. snad.
                return new List<Student>() {new Student() { Result = Result.ErrorFormat("XmlException {0}",xmlex.Message)} };
            }
        }

        public Students GetAllStudentsSorted()
        {
            try
            {
                var xml =
                    XDocument.Load(@"D:\Visual Studio workplace\testFork\ServiceBus\ServiceBus\App_Data\testfile.xml");
                var query = from item in xml.Descendants("student")
                    select new Student()
                    {
                        Id = Guid.Parse(item.Element("id").Value),
                        Name = item.Element("name").Value,
                        Surename = item.Element("surename").Value,
                        City = item.Element("city").Value,
                        Age = (int) item.Element("age"),
                        Result = Result.Success("ok")
                    };

                //...
                var list = query.OrderBy(o=> o.Surename).ToList();

                return new Students() {list = list, Result = Result.Success()};
            }
            catch (ArgumentNullException aex)
            {
                return new Students() { Result = Result.FatalFormat("ArgumentNullException {0}", aex.Message) };
            }
            catch (XmlException xe)
            {
                return new Students() { Result = Result.FatalFormat("XmlException {0}", xe.Message) };
            }
            catch (Exception e)
            {
                return new Students() { Result = Result.FatalFormat("General exception {0}", e.Message) };
            }
        }
        
        public Students GetStudentsByCity(string cityname)
        {
            try
            {
                var xml = XDocument.Load(@"D:\Visual Studio workplace\testFork\ServiceBus\ServiceBus\App_Data\testfile.xml");
                var query = from item in xml.Descendants("student")
                            where item.Element("city").Value == cityname
                            select new Student()
                            {
                                Id = Guid.Parse(item.Element("id").Value),
                                Name = item.Element("name").Value,
                                Surename = item.Element("surename").Value,
                                City = item.Element("city").Value,
                                Age = (int)item.Element("age")
                            };


                return new Students() {list = query.ToList(), Result = Result.Success("Uz asi vim na co je to pluralize.")};
            }
            catch (ArgumentNullException aex)
            {
                return new Students() {Result = Result.FatalFormat("ArgumentNullException {0}",aex.Message)};
            }
            catch (XmlException xe)
            {
                return new Students() {Result = Result.FatalFormat("XmlException {0}",xe.Message)};
            }
            catch (Exception e)
            {
                return new Students() {Result = Result.FatalFormat("General exception {0}", e.Message)};
            }
        }
        /// <summary>
        /// Posle email s kolekci studentu
        /// </summary>
        /// <returns></returns>
        public Result SendStudentsByEmail()
        {
            try
            {
                var mail = new MailMessage(new MailAddress("cer0285@vsb.cz"), new MailAddress("cer0285@vsb.cz"))
                {
                    IsBodyHtml = true
                };

                string html = GetAllStudents().Aggregate("", (current, x) => current + ("Jmeno: " + x.Name + "Prijmeni: " + x.Surename + "ID: " + x.Id + "city" + x.City + "age: " + x.Age + "\n"));

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
