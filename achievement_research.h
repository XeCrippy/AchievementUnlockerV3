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
