TlsTest: App
Das Shalsh am Ende nicht vergessen:
netsh http add urlacl url=http://+:58000/ user=USERNAME listen=yes
oder user=\Everyone

Windows-Firewall: Eingehende Regel 58000 TCP öffenen; Profil: Domäne, Privat

Windows Feature Telnet Client:
telnet http://192.168.178.62 58000

https://datacenteroverlords.com/2012/03/01/creating-your-own-ssl-certificate-authority/
CA-Certificate: openssl genrsa -des3 -out rootCA.key 2048
Self-Sign CA-Certificate: openssl req -x509 -new -nodes -key rootCA.key -sha256 -days 1024 -out rootCA.pem
Windows-Key "Compterzertifikate verwalten" .pem File importieren unter "Vertrauenswürdige Stammzertifizierungsstellen"
Für jeden PC:
openssl genrsa -out device.key 2048
openssl req -new -key device.key -out device.csr
Common Name (eg, YOUR name) []: 192.168.178.45 <- IP des PCs
openssl x509 -req -in device.csr -CA rootCA.pem -CAkey rootCA.key -CAcreateserial -out device.crt -days 500 -sha256
device == device-192-168-178-45
pkcs12 -export -out device-192-168-178-45.pfx -inkey device-192-168-178-45.key -in device-192-168-178-45.crt -certfile device-192-168-178-45.crt
device-192-168-178-45.pfx in  "Computerzertifikate verwalten" als Eigne Zertifikate importiert

netsh http delete urlacl url=http://+:58000/
netsh http add urlacl url=https://+:58000/ user=USERNAME listen=yes

Zertifikat Fingerabdruck (Thumbprint) Compterzertifikate verwalten - Eigene Zertifikate - Doppelklick auf 192.168.178.45 - Fingerabdruck 9d6be07a34e4496b935c14a159cdd4df06272ef8
netsh http add sslcert ipport=192.168.178.45:58000 certhash=9d6be07a34e4496b935c14a159cdd4df06272ef8 appid={a2616933-10cc-417e-b51c-26f8bb92f35d}
(als separate commands: erst netsh, dann http, dann den Rest)

https://docs.microsoft.com/en-us/windows-server/identity/ad-fs/operations/manage-ssl-protocols-in-ad-fs



--------------
Client und Server mit .NetFramework 4.8.4420.0
https://stackoverflow.com/questions/64212994/net-4-8-tls-1-3-issue-on-windows-10:
Ab .NetFramework 4.7: TLS 1.2 (sowie 1.1 und 1.0)
".NET Framework does not support TLS 1.3 yet." => erst ab .net 5
Windows 10 seit Update May 2019: TLS 1.3: Muss aber in der Regitry aktiviert werden

------------------------------------------------------------------------------------------------------
Client-4.8-App-TLS 	Client-System-TLS 	Server-4.8-App-TLS 	Server-System-TLS					        Ergbnis
------------------------------------------------------------------------------------------------------
-				            disabled 1.3			    alles				          disabled: 1.1, 1.2, 1.3		        1.0
1.0				          disabled 1.3			    alles				          disabled: 1.1, 1.2, 1.3		        1.0
1.1				          disabled 1.3			    alles				          disabled: 1.1, 1.2, 1.3		        Fehler
1.2				          disabled 1.3			    alles				          disabled: 1.1, 1.2, 1.3		        Fehler
1.3			            disabled 1.3			    alles				          disabled: 1.1, 1.2, 1.3		        Fehler

-				            disabled 1.3			    1.1					          disabled: 1.1, 1.2, 1.3				    1.0
1.0				          disabled 1.3			    1.2					          disabled: 1.1, 1.2, 1.3				    1.0
1.0				          disabled 1.3			    1.3					          disabled: 1.1, 1.2, 1.3				    1.0

1.3	        		    disabled 1.3			    1.3					          disabled: 1.1, 1.2, 1.3			      Fehler

------------------------------------------------------------------------------------------------------

1.3					        disabled 1.3			    1.3					          disabled: 1.1, 1.2 enabled: 1.3		Fehler

------------------------------------------------------------------------------------------------------

1.3					        enabled 1.3			    1.3					            disabled: 1.1, 1.2 enabled: 1.3		Fehler

------------------------------------------------------------------------------------------------------

1.1                 enabled 1.3         1.3                     disabled: 1.2 enabled: 1.1 1.3    1.1
1.2                 enabled 1.3         1.3                     disabled: 1.2 enabled: 1.1 1.3    Fehler



Mit einer .Net6-Anwendung und gesetzter TLS 1.3 in der APP und mit RegistryEinstellung TLS 1.3 Enabled 1 / DisabledByDefault 0 auf für Server und Client
kommt es dennoch zu Fehlern. Dies scheint aktuell noch ein Bug in der TLS 1.3 Integration in .Net6 zu sein.