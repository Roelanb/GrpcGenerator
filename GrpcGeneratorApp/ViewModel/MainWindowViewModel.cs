using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using GrpcGenerator.Manager;
using GrpcGeneratorApp.Model;
using ICSharpCode.AvalonEdit.Document;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace GrpcGeneratorApp.ViewModel;

public record class UserLoggedIn(string UserName);

[ObservableObject]
public partial class MainWindowViewModel 
{
    [ObservableProperty]
    private string? _firstName = "hello";

    [ObservableProperty]
    public ObservableCollection<SqlTable>? _tableList;

    [ObservableProperty]
    private SqlTable? _selectedItem;

    [ObservableProperty]
    private TextDocument? _protoText;
    [ObservableProperty]
    private TextDocument? _serviceText;


    partial void OnSelectedItemChanged(SqlTable? item)
    {
        if (item == null) return;

      

        var def = new SqlDefinition
        {
            Name = item.Name,
            Package = "adventureWorks2019",
            ServerName = "localhost",
            DatabaseName = "AdventureWorks2019",
            ServiceName = "PersonService",
            ServiceNamespace = "DemoService.Services",
            ConnectionString = "Data Source=(local);Initial Catalog=AdventureWorks2019;Integrated Security=True;",
            ProtoFileLocationServer = null,
            ProtoFileLocationClient = null,
            ServiceFileLocation =null,

            SqlTables = new List<SqlTable>
                {
                    new SqlTable {Catalog = "AdventureWorks2019", Schema =item.Schema, Name = item.Name },
                }

        };

        var pb = new ProtoBuilder();
        var sb = new ServiceBuilder();



        var protoFile = pb.Generate(def);
        var serviceFile = sb.Generate(def);

        //    ProtoText = String.Join(Environment.NewLine, protoFile.GeneratedProtoFile);

        ProtoText = new TextDocument(String.Join(Environment.NewLine, protoFile.GeneratedProtoFile));
        ServiceText = new TextDocument(String.Join(Environment.NewLine, serviceFile.GeneratedServiceFile));

        //foreach (var line in protoFile.GeneratedProtoFile)
        //{
        //    var p = new Paragraph();

        //    p.Inlines.Add(new Run(line));


        //    ProtoText.Text = "hello";
        //}

    }
    public MainWindowViewModel()
    {
      //  WeakReferenceMessenger.Default.Register<UserLoggedIn>(this);
    }

    private bool CanGetTables()
    {
        return FirstName == "hello";
    }

    [RelayCommand(IncludeCancelCommand = true)]
    private async Task GetTables(CancellationToken token)
    {
        try
        {
            using (SqlConnection conn = new SqlConnection("Data Source=(local);Initial Catalog=AdventureWorks2019;Integrated Security=True;"))
            {
                conn.Open();

                var pb = new ProtoBuilder();

                var tables = pb.GetSqlTables(await conn.GetSchemaAsync("Tables"), await conn.GetSchemaAsync("Columns"), null);

                TableList = new ObservableCollection<SqlTable>(tables);

 
            }

            FirstName += " Clicked";
        }
        catch (OperationCanceledException)
        {

            FirstName += "x";
        }

    }
}
