import React, { useState } from 'react';
import axios from 'axios';
import { useAuth } from '../../../components/provider/AuthProvider';
import { TextField, Button, Box, MenuItem, Typography } from '@mui/material';
import { environment } from '../../../environments/environment';
import { useNotification } from '../../../components/provider/NotificationContext';

interface UpdateProfileProps {
  closeModal: () => void;
  refreshProfile: () => void;
}

const UpdateProfile: React.FC<UpdateProfileProps> = ({ closeModal, refreshProfile }) => {
  const { token } = useAuth();
  const { notify } = useNotification();
  const [phoneNumber, setPhoneNumber] = useState('');
  const [height, setHeight] = useState(0);
  const [weight, setWeight] = useState(0);
  const [birthDate, setBirthDate] = useState('');
  const [gender, setGender] = useState('Male');
  const [photo, setPhoto] = useState<File | null>(null);
  const [error, setError] = useState('');
  const apiUrl = `${environment.apiUrl}Auth/profile`;

  const handleSubmit = async (event: React.FormEvent) => {
    event.preventDefault();

    const profileDto = new FormData();
    profileDto.append('phoneNumber', phoneNumber);
    profileDto.append('height', height.toString());
    profileDto.append('weight', weight.toString());
    profileDto.append('birthDate', birthDate);
    profileDto.append('gender', gender);
    if (photo) {
      profileDto.append('photo', photo);
    }

    try {
      await axios.put(apiUrl, profileDto, {
        headers: {
          'Content-Type': 'multipart/form-data',
          Authorization: `Bearer ${token}`
        }
      });
      notify('Profile updated successfully.', 'success');
      refreshProfile();
      closeModal();
    } catch (error) {
      setError('Update failed. Please check your details.');
      notify('Update failed. Please check your details.', 'error');
    }
  };

  return (
    <Box className="container" sx={{ display: 'flex', flexDirection: 'column', alignItems: 'center' }}>
      <Typography variant="h5" component="h2">Update Profile</Typography>
      <form onSubmit={handleSubmit} style={{ width: '100%' }}>
        <TextField
          fullWidth
          margin="normal"
          type="tel"
          name="phoneNumber"
          label="Phone Number"
          value={phoneNumber}
          onChange={(e) => setPhoneNumber(e.target.value)}
          required
        />
        <TextField
          fullWidth
          margin="normal"
          type="number"
          name="height"
          label="Height"
          value={height}
          onChange={(e) => setHeight(parseFloat(e.target.value))}
          required
        />
        <TextField
          fullWidth
          margin="normal"
          type="number"
          name="weight"
          label="Weight"
          value={weight}
          onChange={(e) => setWeight(parseFloat(e.target.value))}
          required
        />
        <TextField
          fullWidth
          margin="normal"
          type="date"
          name="birthDate"
          label="Birth Date"
          value={birthDate}
          onChange={(e) => setBirthDate(e.target.value)}
          required
          InputLabelProps={{ shrink: true }}
        />
        <TextField
          fullWidth
          margin="normal"
          select
          name="gender"
          label="Gender"
          value={gender}
          onChange={(e) => setGender(e.target.value)}
          required
        >
          <MenuItem value="Male">Male</MenuItem>
          <MenuItem value="Female">Female</MenuItem>
        </TextField>
        <Button
          variant="contained"
          fullWidth
          component="label"
        >
          Upload Photo
          <input
            type="file"
            hidden
            onChange={(e) => setPhoto(e.target.files ? e.target.files[0] : null)}
          />
        </Button>
        {error && <Typography color="error">{error}</Typography>}
        <Box sx={{ display: 'flex', justifyContent: 'space-between', mt: 2 }}>
          <Button type="submit" variant="contained">Update</Button>
        </Box>
      </form>
    </Box>
  );
};

export default UpdateProfile;

