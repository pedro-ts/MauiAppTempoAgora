using MauiAppTempoAgora.Models;
using MauiAppTempoAgora.Services;

namespace MauiAppTempoAgora
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txt_cidade.Text))
                {
                    Tempo? t = await DataService.GetPrevisao(txt_cidade.Text);

                    if (t != null)
                    {
                        string dados_previsao = "";

                        // Dados já existentes
                        dados_previsao = $"Latitude: {t.lat} \n" +
                                         $"Longitude: {t.lon} \n" +
                                         $"Nascer do Sol: {t.sunrise} \n" +
                                         $"Por do Sol: {t.sunset} \n" +
                                         $"Temp Máx: {t.temp_max} \n" +
                                         $"Temp Min: {t.temp_min} \n";

                        // >>> MODIFICAÇÕES: Inclusão dos novos dados solicitados <<<
                        dados_previsao += $"Descrição: {t.description} \n";         // Adiciona a descrição textual do clima
                        dados_previsao += $"Velocidade do Vento: {t.speed} \n";       // Adiciona a velocidade do vento
                        dados_previsao += $"Visibilidade: {t.visibility} \n";         // Adiciona a visibilidade

                        lbl_res.Text = dados_previsao;
                    }
                    else
                    {
                        lbl_res.Text = "Sem dados de Previsão";
                    }
                }
                else
                {
                    lbl_res.Text = "Preencha a cidade.";
                }
            }
            // >>> MODIFICAÇÕES: Tratamento específico para erros HTTP <<<
            // Captura exceções lançadas por requisições HTTP e verifica o StatusCode
            catch (HttpRequestException ex)
            {
                // Se o status code for 404, indica que o nome da cidade não foi encontrado
                if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    lbl_res.Text = "Cidade não encontrada.";
                }
                // Se o status code for RequestTimeout, possivelmente não há conexão com a internet
                else if (ex.StatusCode == System.Net.HttpStatusCode.RequestTimeout)
                {
                    await DisplayAlert("Erro", "Sem conexão com a internet.", "OK");
                }
                else
                {
                    await DisplayAlert("Ops", ex.Message, "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ops", ex.Message, "OK");
            }
        }

    }

}
