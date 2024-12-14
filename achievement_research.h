void search_memory(void* start_address, size_t length, uint32_t target_value, uint32_t target_value_2) {
		uint8_t* mem = (uint8_t*)start_address;
		uint32_t test = 0x82000000;

		for (size_t i = 0; i < length - 3; i++) {
			if (*((uint32_t*)(mem + i)) == target_value) {
				if (i >= 8 && *((uint32_t*)(mem + i - 8)) == target_value_2) {
					std::ostringstream oss;
					oss << "Found XUserWriteAchievements at offset: 0x" << std::hex << test + i - 0x24;
					std::string result = oss.str();
					Utilities::Xam::XNotify(result);
					break;
				}
			}
		}
	}

	void achievementUnlocker::LocateXUserWriteAchievements() {
	     search_memory((void*)0x82000000, 0x1000000, 0x60840008, 0x38e00008);
	}

//
//
//
typedef struct _XUSER_ACHIEVEMENT {
    DWORD dwAchievementId;   // Unique ID for the achievement
    DWORD dwUserIndex;
} XUSER_ACHIEVEMENT, *PXUSER_ACHIEVEMENT;

//Example function to demonstrate usage of XUserWriteAchievements
void Testing() {
    // Number of achievements to write
    const DWORD numAchievements = 1;

    // Define an example achievement
    XUSER_ACHIEVEMENT achievement = {};
    achievement.dwAchievementId = 1;           // The achievement ID
    achievement.dwUserIndex = 0;

    // Create an array of achievements (even for one achievement)
    XUSER_ACHIEVEMENT achievements[] = { achievement };

    // Optional: Use an OVERLAPPED structure for asynchronous handling
    XOVERLAPPED overlapped = {};
    PXOVERLAPPED pOverlapped = nullptr; // Pass nullptr for synchronous operation
    // If asynchronous, use &overlapped.

    // Call the function to write achievements
    DWORD result = XUserWriteAchievements(numAchievements, achievements, pOverlapped);

    if (result == ERROR_SUCCESS) {
        Utilities::Xam::XNotify("Achievement(s) written successfully!");
    } else {
        Utilities::Xam::XNotify("Failed to write achievement(s).");
    }
}
