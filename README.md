# Tariff Switch Processing (.NET)

Eine mehrschichtige .NET-Anwendung, die Tarifwechsel-Anfragen aus CSV-Dateien einliest,
entscheidet und ggf. Folgeaktionen erzeugt (z. B. Smart-Meter-Upgrade).

## Funktionen

- Verarbeitung von Tarifwechsel-Requests (`requests.csv`)
- Prüfung von Kunden- und Tarifdaten
- Idempotente Verarbeitung (keine doppelten Requests)
- Entscheidungen: Approved / Rejected
- Automatische Follow-Up-Action, wenn Smart Meter erforderlich ist
- Berechnung von Deadlines basierend auf SLA und Zusatzstunden
- Speicherung von Follow-Ups und bereits verarbeiteten Requests

## SLA-Konfiguration

Die Fristen sind zentral konfiguriert:

- Standard: 48h  
- Premium: 24h  
- Smart-Meter-Upgrade: +12h  

Anpassbar über `SlaOptions` in `CompositionRoot.cs`.

## Projektstruktur

- DomainModel/        Domänenobjekte inkl. fachlicher Regeln und Entscheidungen
- Application/        Use Case Logik, Koordination des Ablaufs
- Infrastructure/     CSV-Handling, Ausgabe, Dateispeicher
- ConsoleApp/         Einstiegspunkt
- *Tests/ Unit-Tests


## CSV Input Files

Im Ordner:

ConsoleApp/InputFiles/


Erforderliche Dateien:

- `customers.csv`
- `tariffs.csv`
- `requests.csv`
- `processed.txt` (wird automatisch erstellt)
- `followups.csv` (wird automatisch erstellt)
