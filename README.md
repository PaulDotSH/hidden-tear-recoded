# Hidden Tear Recoded
This is a recode of the first open source ransomware called "Hidden-Tear"
Almost every "flaw" the original Hidden Tear had, is fixed in this recode

##### Features Include:
- Encrypts all files from the computer
- Tries to uncheck "read-only" file attribute
- Uses a ransom salt
- Sends encryption key, salt, computer and ip details to a backend (as JSON)
- Files are decryptable with the decryptor
- Uses a cryptographically secure random generator (files CANNOT be decrypted w/o the key and salt)
- Generates a thread for all drives (so encryption is faster)
- Much more

### LEGAL DISCLAIMER PLEASE READ!
##### I am not responsible for any actions and or damages caused by this software. You bear the full responsibility of your actions and acknowledge that this software was created for educational purposes only. This software's intended purpose is NOT to be used maliciously, or on any system that you do not have own or have explicit permission to operate and use this program on. By using this software, you automatically agree to the LICENSE.

This project is licensed under the GPL 3.0 License - see the [LICENSE](/LICENSE) file for details

EDIT: the only problem this software might have, is that depending on how "secure" crypto secure is in c#, the key could technically be "reversed", however this has an easy fix, generate the key on the backend.
