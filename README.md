

🌸 Labb 3 – Quiz Application 🌸

En WPF-applikation för att skapa, redigera och spela quiz!

Av: Vendela
Kurs: .NET / Utveckling i C# 
Repo: https://github.com/Vendela123/Labb3


💗 ✨ Beskrivning

Detta projekt är en WPF-baserad quiz-applikation där användaren kan:

· Skapa egna frågepaket (Question Packs)

· Lägga till / ta bort frågor

· Redigera pack-inställningar (svårighet, tid per fråga m.m.)

· Importera frågor via Open Trivia DB API

· Spela quizet med shufflade frågor och svarsalternativ

· Få feedback visuellt (rätt/fel ikon)

· Se resultat efter avslutat quiz

Allt sparas automatiskt i JSON-format i AppData.

Funktioner


🩷 1. Skapa och hantera frågor

Skapa nya frågepaket

Lägg till frågor med 4 svarsalternativ

Välj vilket alternativ som är rätt


🩷 2. Importera frågor via API

Applikationen använder Open Trivia DB för att hämta:

Kategorier

Svårighetsgrader

Valfritt antal frågor

All kommunikation sker asynkront.


🩷 3. Inbyggd Quiz-spelare

Nedräkning per fråga

Slumpade svarsalternativ

Visar rätt/fel efter knapptryck

Hoppar vidare automatiskt


🩷 4. Resultatskärm

Efter quizet visas:

Antal rätt

Antal fel

Total score

Möjlighet att återgå till redigeraren


🌸 Tekniker & Verktyg
Teknik	Användning
C# .NET 8	Logik & MVVM-struktur
WPF	UI • XAML
MVVM	Ren kod & bindningar
JSON-lagring	Spara frågepaket
Open Trivia API	Importera frågor
IValueConverter	Rätt/Fel-indikatorer
RelayCommand	Kommandon




🎀 Installation

1️⃣ Klona repot

git clone https://github.com/Vendela123/Labb3

2️⃣ Öppna i Visual Studio

Öppna .sln-filen

Välj Debug

Tryck Start

3️⃣ Kör appen 🎉

Ingen setup krävs — JSON-filer skapas automatiskt i AppData.


💞 Hur man spelar quizet

Skapa ett nytt frågepaket

Lägg till frågor

Tryck Play (Ctrl + P)

Välj dina svar

Se resultat efter sista frågan


🌷 Tangentbordskommandon
Shortcut	Funktion
Ctrl + P	Starta quiz
Ctrl + E	Gå till edit-läge
Ctrl + O	Pack options
Insert	Lägg till fråga
Delete	Ta bort fråga


👩‍💻 Developer: Vendela Magnusson

