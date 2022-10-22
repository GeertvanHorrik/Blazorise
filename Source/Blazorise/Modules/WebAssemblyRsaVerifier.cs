﻿#region Using directives
using System.Text;
using System;
using System.Threading.Tasks;
using Blazorise.Licensing.Signing;
using Microsoft.JSInterop;
#endregion

namespace Blazorise.Modules
{
    /// <summary>
    /// Default implementation of the RSA JS module.
    /// </summary>
    internal class WebAssemblyRsaVerifier : BaseJSModule, /*IJSRsa,*/ IVerifier
    {
        #region Members

        private readonly string publicKey;

        private readonly byte[] publicKeyBytes;

        #endregion

        #region Constructors

        /// <summary>
        /// Default module constructor.
        /// </summary>
        /// <param name="jsRuntime">JavaScript runtime instance.</param>
        /// <param name="versionProvider">Version provider.</param>
        public WebAssemblyRsaVerifier( IJSRuntime jsRuntime, IVersionProvider versionProvider, string publicKey )
            : base( jsRuntime, versionProvider )
        {
            this.publicKey = publicKey;
            this.publicKeyBytes = Convert.FromBase64String( publicKey );
        }

        #endregion

        #region Methods

        public async Task<bool> Verify( string content, string signature )
        {
            //No idea why... but the content here is not right. Newline is represented by \n. While in the original it is by \r\n. 
            //Let's just run a replace for now...
            content = content.Replace( "\n", "\r\n" );

            var bytesSignature = Convert.FromBase64String( signature );
            UnConfuse( bytesSignature );
            signature = Convert.ToBase64String( bytesSignature );

            var result = await InvokeSafeAsync<bool>( "verify", publicKey, content, signature );

            return result;
        }

        //The UnConfuse should be centralized? I have seen it in 3 separate places, Im CONFUSED!!
        private static void UnConfuse( byte[] bytes )
        {
            for ( int i = 0; i < bytes.Length; i++ )
            {
                bytes[i] ^= Blazorise.Licensing.Constants.ConfusingBytes[i % Blazorise.Licensing.Constants.ConfusingBytes.Length];
            }
        }

        #endregion

        #region Properties

        /// <inheritdoc/>
        public override string ModuleFileName => $"./_content/Blazorise/rsa.js?v={VersionProvider.Version}";

        #endregion        
    }
}
