using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Killerrin_Studios_Toolkit
{
    public class CortanaTools
    {
        public static async void UpdateCortanaVDFile(Uri voiceDefinitionFile)
        {
            var storageFile = await Windows.Storage.StorageFile.GetFileFromApplicationUriAsync(voiceDefinitionFile);
            await Windows.ApplicationModel.VoiceCommands.VoiceCommandDefinitionManager.InstallCommandDefinitionsFromStorageFileAsync(storageFile);
        }
    }
}
