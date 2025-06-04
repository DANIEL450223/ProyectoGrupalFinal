using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using ProyectoGrupalP2.Models;

namespace ProyectoGrupalP2.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService()
        {
            
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7046/api/")
            };
        }

        
        public async Task<List<Usuario>> GetUsuariosAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Usuario>>("usuarios");
        }

        public async Task<Usuario> PostUsuarioAsync(Usuario u)
        {
            
            var resp = await _httpClient.PostAsJsonAsync("usuarios", u);
            if (!resp.IsSuccessStatusCode)
                return null;

            
            return await resp.Content.ReadFromJsonAsync<Usuario>();
        }


        public async Task<bool> DeleteUsuarioAsync(int id)
        {
            var resp = await _httpClient.DeleteAsync($"usuarios/{id}");
            return resp.IsSuccessStatusCode;
        }

        
        public async Task<List<Estacionamiento>> GetEstacionamientosAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Estacionamiento>>("estacionamientos");
        }

        public async Task<bool> UpdateEstacionamientoAsync(Estacionamiento e)
        {
            var resp = await _httpClient.PutAsJsonAsync($"estacionamientos/{e.Id}", e);
            return resp.IsSuccessStatusCode;
        }

        public async Task<List<Historial>> GetHistorialAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Historial>>("historial");
        }

        public async Task<bool> PostHistorialAsync(Historial h)
        {
            var resp = await _httpClient.PostAsJsonAsync("historial", h);
            return resp.IsSuccessStatusCode;
        }
    }
}
