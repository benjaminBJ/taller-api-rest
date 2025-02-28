namespace csc_srv_carpeta_digital.Exceptions;

public class AuthException : Exception
{
    public AuthException(string v) : base("credenciales invalidas")
    {

    }
}
