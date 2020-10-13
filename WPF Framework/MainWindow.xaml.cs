using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPF_Framework
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {

            string dominio = @"global.baxter.com";
            string usuario = txUsuario.Text.Trim();
            //string contraseña = txContraseña.Text.Trim();
            string contraseña = PassBox.Password;


            //string suma = usuario + contraseña;
            // Resultado.Text =  suma;
            using (PrincipalContext principalContext = new PrincipalContext(ContextType.Domain, dominio))
            {


                bool userValid = principalContext.ValidateCredentials(usuario, contraseña);


                if (userValid == true)
                {

                    Resultado2.Text = (userValid + "    " + "*******validacion de usuario correcta**********");


                    using (UserPrincipal user = UserPrincipal.FindByIdentity(principalContext, usuario))
                    {
                        List<string> result = new List<string>();
                        WindowsIdentity wi = new WindowsIdentity(usuario);


                        foreach (IdentityReference group in wi.Groups)
                        {
                            try
                            {
                                result.Add(group.Translate(typeof(NTAccount)).ToString());
                            }
                            catch (Exception ex) { }
                        }

                        result.Sort();
                        bool testx = false;
                        foreach (var t in result)
                        {
                            if (t.Equals("GLOBAL\\ESSA-HojaResumen_Users"))
                            {
                                testx = true;
                                Resultado3.Text = "";
                                Resultado.Text = "Accesso Permitido" + "\n  " + "El usuario pertenece al grupo: " + "\n" + (t) + " \n  " + "******Validacion de grupo correcta*********";


                            }
                        }

                        if (testx != true)
                        {
                            Resultado3.Text = "";
                            Resultado.Text = "Accesso Denegado" + "\n" + "No tiene suficientes Permisos" + "\n" + "Su usuario no pertenece al grupo ESSA-HojaResumen_Users ";
                            Resultado2.Text = "";
                            MessageBox.Show("Accesso Denegado");


                        }







                    }

                }
                else
                {
                    Resultado3.Text = "Usuario y/o Contraseña Incorrecto";
                    Resultado.Text = "";
                    Resultado2.Text = "";
                }

            }
        }
    


                private void Resultado_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
