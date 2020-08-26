using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Constants;
using DAL.AzureStorage.Interfaces;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Threading.Tasks;

namespace DAL.AzureStorage
{
	public class AzureStorageRepository : IAzureStorageRepository
	{
		private readonly string _connectionString;
		private readonly string _blobContainerName;
		private readonly IConfiguration _configuration;
		public AzureStorageRepository(IConfiguration configuration)
		{
			_configuration = configuration;
			_connectionString = _configuration.GetValue<string>(ConfigurationKeys.AzureStorageConnectionString) ?? _configuration.GetValue<string>(ConfigurationKeys.LocalAzureStorageConnectionString);
			_blobContainerName = _configuration.GetValue<string>(ConfigurationKeys.AzureStorageBlobContainerName);
			InitializeContainer().Wait();
		}

		private async Task InitializeContainer()
		{
			var blobContainer = new BlobContainerClient(_connectionString, _blobContainerName);
			blobContainer.CreateIfNotExists();

			var directory = new DirectoryInfo("Resources");
			var files = directory.GetFiles();
			foreach (var file in files)
			{
				var name = Path.GetFileNameWithoutExtension(file.Name).ToLower();
				if (!await CheckImageExistance(name))
				{
					using (var stream = file.OpenRead())
					{
						await blobContainer.UploadBlobAsync(name, stream);
					}
				}
			}
		}

		public async Task AddImage(string name, byte[] content)
		{
			var blobContainer = new BlobContainerClient(_connectionString, _blobContainerName);
			using (var stream = new MemoryStream(content))
			{
				await blobContainer.UploadBlobAsync(name, stream);
			}
		}

		public async Task<byte[]> GetImage(string name)
		{
			var blobContainer = new BlobContainerClient(_connectionString, _blobContainerName);
			var blobClient = blobContainer.GetBlobClient(name);
			var blob = await blobClient.DownloadAsync();
			using (var stream = new MemoryStream())
			{
				await blob.Value.Content.CopyToAsync(stream);
				return stream.ToArray();
			}
		}
		
		public async Task<bool> CheckImageExistance(string name)
		{
			var blobContainer = new BlobContainerClient(_connectionString, _blobContainerName);
			var blobClient = blobContainer.GetBlobClient(name);
			return await blobClient.ExistsAsync();
		}

		public async Task RemoveImage(string name)
		{
			var blobContainer = new BlobContainerClient(_connectionString, _blobContainerName);
			var blobClient = blobContainer.GetBlobClient(name);
			await blobClient.DeleteAsync(DeleteSnapshotsOption.IncludeSnapshots);
		}
	}
}
