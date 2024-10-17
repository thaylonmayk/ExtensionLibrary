# EnumExtensions

With EnumExtensions, you can effortlessly convert enumerations to dictionaries, retrieve all enumeration values and names, and perform various other advanced operations, ensuring a more efficient and effective development process.


## Instalação

Você pode instalar a biblioteca via NuGet. No seu terminal, execute:

```bash
dotnet add package TL.EnumExtensionsLibrary
```

## Usage

Once you have installed the Enum extension library within your project. Enum extensions functions will be available on all string types 

```
            public enum SystemErrorCodes
            {
                [Description("An unknown error occurred.")]
                Unknown,
                [Description("The system is out of memory.")]
                OutOfMemory,
                [Description("An invalid parameter was provided.")]
                InvalidParameter,
                [Description("An application error occurred.")]
                Application,
                [Description("A business error occurred.")]
                Business,
                [Description("A database error occurred.")]
                Database
            }

            public enum Priority
            {
                Urgent,
                High,
                Medium,
                Low
            }

            IList<Priority> valoresPriority = Enum.GetValues<Priority>();
            Console.WriteLine("Valores do enum Cores:");
            foreach (var cor in valoresPriority)
            {
                Console.WriteLine(cor);
            }

            IList<string> nomesPriority = Enum.GetNames<Priority>();
            Console.WriteLine("\nNomes do enum Cores:");
            foreach (var nome in nomesPriority)
            {
                Console.WriteLine(nome);
            }
            Console.WriteLine(SystemErrorCodes.Application.GetDescription<SystemErrorCodes>());

            Console.WriteLine(Priority.Parse<Priority>("Urgent"));