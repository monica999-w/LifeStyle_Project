import React, { useState, useEffect } from 'react';
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
  const [height, setHeight] = useState<number | null>(null);
  const [weight, setWeight] = useState<number | null>(null);
  const [birthDate, setBirthDate] = useState('');
  const [gender, setGender] = useState('');
  const [photo, setPhoto] = useState<File | null>(null);
  const [error, setError] = useState('');
  const apiUrl = `${environment.apiUrl}Auth/profile`;

  useEffect(() => {
    const fetchProfile = async () => {
      try {
        const response = await axios.get(apiUrl, {
          headers: {
            Authorization: `Bearer ${token}`
          }
        });
        const { phoneNumber, height, weight, birthDate, gender } = response.data;
        setPhoneNumber(phoneNumber);
        setHeight(height);
        setWeight(weight);
        // Normalize the birthDate to eliminate timezone differences
        setBirthDate(new Date(birthDate).toISOString().split('T')[0]);
        setGender(gender);
      } catch (error) {
        setError('Failed to fetch profile data.');
        notify('Failed to fetch profile data.', 'error');
      }
    };

    fetchProfile();
  }, [apiUrl, token, notify]);

  const handleSubmit = async (event: React.FormEvent) => {
    event.preventDefault();

    const profileDto = new FormData();
    if (phoneNumber) profileDto.append('phoneNumber', phoneNumber);
    if (height !== null) profileDto.append('height', height.toString());
    if (weight !== null) profileDto.append('weight', weight.toString());
    if (birthDate) profileDto.append('birthDate', new Date(birthDate).toISOString());
    if (gender) profileDto.append('gender', gender);
    if (photo) profileDto.append('photoUrl', photo);

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
    <Box
      className="container"
      sx={{
        display: 'flex',
        flexDirection: 'column',
        alignItems: 'center',
        padding: 3,
        backgroundColor: '#f7f7f7',
        borderRadius: 2,
        boxShadow: 1,
        maxWidth: 500,
        margin: '0 auto'
      }}
    >
      <Typography variant="h5" component="h2" sx={{ marginBottom: 2 }}>
        Update Profile
      </Typography>
      <form onSubmit={handleSubmit} style={{ width: '100%' }}>
        <TextField
          fullWidth
          margin="normal"
          type="tel"
          name="phoneNumber"
          label="Phone Number"
          value={phoneNumber}
          onChange={(e) => setPhoneNumber(e.target.value)}
        />
        <TextField
          fullWidth
          margin="normal"
          type="number"
          name="height"
          label="Height"
          value={height !== null ? height : ''}
          onChange={(e) => setHeight(parseFloat(e.target.value))}
        />
        <TextField
          fullWidth
          margin="normal"
          type="number"
          name="weight"
          label="Weight"
          value={weight !== null ? weight : ''}
          onChange={(e) => setWeight(parseFloat(e.target.value))}
        />
        <TextField
          fullWidth
          margin="normal"
          type="date"
          name="birthDate"
          label="Birth Date"
          value={birthDate}
          onChange={(e) => setBirthDate(e.target.value)}
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
        >
          <MenuItem value="Male">Male</MenuItem>
          <MenuItem value="Female">Female</MenuItem>
        </TextField>
        <Button
          variant="contained"
          fullWidth
          component="label"
          sx={{ marginTop: 2 }}
        >
          Upload Photo
          <input
            type="file"
            hidden
            onChange={(e) => setPhoto(e.target.files ? e.target.files[0] : null)}
          />
        </Button>
        {error && <Typography color="error" sx={{ marginTop: 2 }}>{error}</Typography>}
        <Box sx={{ display: 'flex', justifyContent: 'space-between', marginTop: 2 }}>
          <Button type="submit" variant="contained" sx={{ marginRight: 1 }}>
            Update
          </Button>
          <Button variant="outlined" onClick={closeModal}>
            Cancel
          </Button>
        </Box>
      </form>
    </Box>
  );
};

export default UpdateProfile;

