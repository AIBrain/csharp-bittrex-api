
namespace Bittrex {
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Net;
    using System.Text;
    using Newtonsoft.Json;

    public class ApiCall {
        private Boolean Simulate { get; }

        public ApiCall( Boolean simulate ) {
            this.Simulate = simulate;
        }

        public T CallWithJsonResponse<T>( String uri, Boolean hasEffects, params Tuple<String, String>[] headers ) {
            if ( this.Simulate && hasEffects ) {
                Debug.WriteLine( "(simulated)" + GetCallDetails( uri ) );
                return default( T );
            }

            Debug.WriteLine( GetCallDetails( uri ) );
            var request = HttpWebRequest.CreateHttp( uri );
            foreach ( var header in headers ) {
                request.Headers.Add( header.Item1, header.Item2 );
            }

            using ( var response = request.GetResponse() as HttpWebResponse ) {
                if ( null == response ) {
                    throw new NullReferenceException( nameof( response ) );
                }
                if ( response.StatusCode == HttpStatusCode.OK ) {
                    var responseStream = response.GetResponseStream();
                    if ( responseStream == null ) {
                        throw new NullReferenceException( nameof( responseStream ) );
                    }
                    using ( var sr = new StreamReader( responseStream ) ) {
                        var content = sr.ReadToEnd();
                        var jsonResponse = JsonConvert.DeserializeObject< ApiCallResponse< T > >( content );

                        if ( jsonResponse.success ) {
                            return jsonResponse.result;
                        }

                        throw new Exception( jsonResponse.message + "Call Details=" + GetCallDetails( uri ) );
                    }
                }
                else {
                    throw new Exception( "Error - StatusCode=" + response.StatusCode + " Call Details=" + GetCallDetails( uri ) );
                }
            }
        }

        private static String GetCallDetails( String uri ) {
            var sb = new StringBuilder();
            var u = new Uri( uri );
            sb.Append( u.AbsolutePath );
            if ( !u.Query.StartsWith( "?" ) ) {
                return sb.ToString();
            }

            var queryParameters = u.Query.Substring( 1 ).Split( '&' );
            foreach ( var p in queryParameters ) {
                var lowerP = p.ToLower();
                if ( lowerP.StartsWith( "api" ) || lowerP.StartsWith( "nonce" ) ) {
                    continue;
                }
                var kv = p.Split( '=' );
                if ( kv.Length == 2 ) {
                    if ( sb.Length != 0 ) {
                        sb.Append( ", " );
                    }

                    sb.Append( kv[ 0 ] ).Append( " = " ).Append( kv[ 1 ] );
                }
            }
            return sb.ToString();
        }
    }
}
