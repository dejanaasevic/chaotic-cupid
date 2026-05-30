using System;
using System.Linq;
using ChaoticCupid.Shared.Models;
using Microsoft.AspNetCore.SignalR.Client;

// add connection to the signalR hub
var connection = new HubConnectionBuilder().WithUrl("https://localhost:7050/cupid").Build();

// method to observe incoming letters from the server
connection.On<LetterDTO>("ReceiveLetter", async (LetterDTO) =>
{
    Console.WriteLine($"New letter from {LetterDTO.FromUsername}, {LetterDTO.FromCity}, age {LetterDTO.FromAge}:\n");
    Console.WriteLine($"{LetterDTO.Message}\n");

    if (LetterDTO.ShowPhone)
    {
        Console.WriteLine($"Contact number: {LetterDTO.FromPhone}\n");
    }
});

await connection.StartAsync();
Console.WriteLine("Connected to Chaotic Cupid successfully!");

// enter and validate user details for registration
Console.WriteLine("Welcome to Chaotic Cupid 💌! Let's get you registered.");

string username;
do
{
    Console.Write("Enter your username: ");
    username = Console.ReadLine();
    if (string.IsNullOrEmpty(username))
    {
        Console.WriteLine("Username cannot be empty. Please try again.");
    }
} while (string.IsNullOrEmpty(username));

string city;
do
{
    Console.Write("Enter your city: ");
    city = Console.ReadLine();
    if (string.IsNullOrEmpty(city))
    {
        Console.WriteLine("City cannot be empty. Please try again.");
    }
} while (string.IsNullOrEmpty(city));

int age;
string ageInput;
do
{
    Console.Write("Enter your age: ");
    ageInput = Console.ReadLine();
    if (!int.TryParse(ageInput, out age) || age <= 0)
    {
        Console.WriteLine("Invalid age. Please enter a positive number.");
    }
} while (!int.TryParse(ageInput, out age) || age <= 0);

string phoneNumber;
do
{
    Console.Write("Enter your phone number: ");
    phoneNumber = Console.ReadLine();
    if (string.IsNullOrEmpty(phoneNumber) || phoneNumber.Any(char.IsLetter))
    {
        Console.WriteLine("Invalid phone number. Please enter digits only.");
    }
} while (string.IsNullOrEmpty(phoneNumber) || phoneNumber.Any(char.IsLetter));

// create a PersonDTO object and send it to the server for registration
PersonDTO personDTO = new PersonDTO(username, city, age, phoneNumber);
await connection.InvokeAsync("InitSinglePerson", personDTO);

Console.WriteLine("Registration successful! You are now connected 💌.");
Console.WriteLine("Type /block <username> to block someone, or press Enter to confirm a received letter.");

while (true)
{
    string command = Console.ReadLine();
    if (command.StartsWith("/block "))
    {
        // block a user by sending the block command to the server with the username to block
        string usernameToBlock = command.Substring(7, command.Length - 7).Trim();
        if (!string.IsNullOrEmpty(usernameToBlock))
        {
            await connection.InvokeAsync("BlockUser", usernameToBlock);
            Console.WriteLine($"You have blocked {usernameToBlock}.");
        }
    }
    else if (string.IsNullOrEmpty(command))
    {
        // confirm tha a letter has been received by sending the confirmation command to the server
        await connection.InvokeAsync("ConfirmLetterReceived");
    }
    else
    {
        Console.WriteLine("Invalid command. Please try again.");
    }
}
