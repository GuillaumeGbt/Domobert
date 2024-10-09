from datetime import datetime, timedelta
import random

# Fonction pour générer des données réalistes de température et d'humidité
def generate_large_temp_humi_data(start_date, end_date, device_id):
    data = []
    current_date = start_date

    while current_date <= end_date:
        temperature = round(random.uniform(18.0, 25.0), 2)  # Température en °C
        humidity = round(random.uniform(30.0, 60.0), 2)  # Humidité en %
        timestamp = current_date.strftime('%Y-%m-%d %H:%M:%S')
        data.append((temperature, humidity, timestamp, device_id))
        current_date += timedelta(minutes=30)  # Enregistrement toutes les 30 minutes

    return data

# Paramètres de génération de données
start_date = datetime(2024, 9, 15)  # Début de la période
end_date = datetime(2024, 10, 9)   # Fin de la période
device_id = 6  # ID de l'appareil

# Génération d'un lot de données
large_data_batch = generate_large_temp_humi_data(start_date, end_date, device_id)

# Afficher ou exporter le résultat sous forme de script SQL
output_filename = "insert_data.sql"

with open(output_filename, "w") as f:
    for record in large_data_batch:
        f.write(f"INSERT INTO History_TempHumi (Temperature, Humidity, Timestamp, DeviceID)\n")
        f.write(f"VALUES ({record[0]}, {record[1]}, '{record[2]}', {record[3]});\n")

print(f"Les données ont été générées et sauvegardées dans {output_filename}")