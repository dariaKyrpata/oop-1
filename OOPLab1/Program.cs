using FileStorage;

PersonService service = new PersonService();

PersonRepository repository = new PersonRepository();

ITextFileStore fileStore = new StreamTextFileStore();

ConsoleMenu menu = new ConsoleMenu(
    service,
    repository,
    fileStore
);

menu.Run();