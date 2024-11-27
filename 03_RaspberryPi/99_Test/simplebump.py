#------------------------------------------------*
# Autor: Noah Gerstlauer                          |
# Department: THGM-TL1                            |
# Email: Noah.Gerstlauer@airbus.com               |
# Date: 2023-09                                   |
#------------------------------------------------*/

# Gibt alle auf dem entsprechenden Port empfangenen Daten aus. (Kommunikationstest)

import socket

HOST = ''
PORT = 4444

server_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
server_socket.bind((HOST, PORT))
server_socket.listen(5)

own_ip = socket.gethostbyname(socket.gethostname())
print(f"Server lauscht auf {own_ip}:{PORT}")

while True:
    client_socket, addr = server_socket.accept()
    print(f"Verbindung von {addr} hergestellt")

    while True:
        data = client_socket.recv(16)
        if not data:
            break

        received_data = ','.join(str(byte) for byte in data)
        print(f"Empfangene Daten: {received_data}")
    client_socket.close()

